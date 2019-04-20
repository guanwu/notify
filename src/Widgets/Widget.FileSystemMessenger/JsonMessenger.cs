using Guanwu.Toolkit.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Guanwu.Notify.Widget.FileSystemMessenger.Json
{
    [Serializable]
    public sealed class JsonMessenger : MarshalByRefObject, IPipelineMessenger
    {
        public string WidgetName => Const.WIDGET_NAME;

        public event PipelineMessageEventHandler OnMessageReceived;
        public event PipelineMessageEventHandler OnMessagePersisting;
        public event PipelineMessageEventHandler OnMessagePersisted;

        private ILogger logger = default;
        private FileWatcher FileWatcher = default;

        public void Initialize(ILogger logger = default)
        {
            this.logger = logger ?? NullLogger.Instance;
            this.logger.LogInformation($"[MESSENGER_ACTIVATED] {WidgetName}");

            try
            {
                Directory.CreateDirectory(Const.MESSAGE_DIR);

                FileWatcher = new FileWatcher
                {
                    Directory = Const.MESSAGE_DIR,
                    Filter = Const.FILTER
                };

                FileWatcher.OnFileCreated += OnFileCreatedAsync;
                FileWatcher.Start();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
            }
        }

        private void OnFileCreatedAsync(object sender, FileWatcherInfo e)
        {
            if (OnMessageReceived == null) return;
            if (e == null) return;

            Task.Run(() => TryBackup(e));
            Task.Run(() => TryPushMessage(e));
        }

        private void TryBackup(FileWatcherInfo info)
        {
            try
            {
                string backupDir = Path.Combine(Const.MESSAGE_DIR, DateTime.Now.ToString(Const.BACKUP_PATTERN));
                Directory.CreateDirectory(backupDir);

                string backupPath = Path.Combine(backupDir, info.Name);
                File.Delete(backupPath);
                File.Move(info.FullName, backupPath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        private void TryPushMessage(FileWatcherInfo info)
        {
            try
            {
                var context = new Dictionary<string, string>
                {
                    { WidgetConst.PMSG_ID, Path.GetFileNameWithoutExtension(info.Name) },
                    { WidgetConst.PMSGR_ID, WidgetName }
                };

                Parallel.ForEach(OnMessageReceived.GetInvocationList(), t =>
                {
                    try
                    {
                        ((PipelineMessageEventHandler)t).BeginInvoke(info.FileBytes, context, null, null);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

    }
}
