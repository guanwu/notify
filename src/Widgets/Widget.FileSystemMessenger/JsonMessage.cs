using Guanwu.Toolkit.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Guanwu.Notify.Widget.FileSystemMessenger.Message
{
    [Serializable]
    public sealed class JsonMessage : MarshalByRefObject, IPipelineMessenger
    {
        public string WidgetName => Const.WIDGET_NAME;

        public event PipelineMessageEventHandler OnMessageReceived;
        public event PipelineMessageEventHandler OnMessagePersisting;
        public event PipelineMessageEventHandler OnMessagePersisted;

        private ILogger Logger = default;
        private FileWatcher FileWatcher = default;

        public void Initialize(ILogger logger = default)
        {
            Logger = logger ?? NullLogger.Instance;
            Logger.LogInformation($">>>> {WidgetName} <<<<");

            try {
                Directory.CreateDirectory(Const.DIRECTORY);
                FileWatcher = new FileWatcher {
                    Directory = Const.DIRECTORY,
                    Filter = Const.FILTER
                };
                FileWatcher.OnFileCreated += OnFileCreatedAsync;
                FileWatcher.Start();
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }
        }

        private void OnFileCreatedAsync(object sender, FileWatcherInfo e)
        {
            if (OnMessageReceived == null) return;
            if (e == null) return;

            Task.Run(() => TryBackup(e));
            Task.Run(() => TryPushMessage(e));
        }

        private void TryPushMessage(FileWatcherInfo info)
        {
            try {
                var context = new Dictionary<string, string> {
                    { WidgetConst.PMSG_ID, Path.GetFileNameWithoutExtension(info.Name) },
                    { WidgetConst.PMSG_SOURCE, WidgetName }
                };

                Parallel.ForEach(OnMessageReceived.GetInvocationList(), t => {
                    ((PipelineMessageEventHandler)t).BeginInvoke(info.FileBytes, context, null, null);
                });
            }
            catch (AggregateException e) {
                foreach (var ie in e.InnerExceptions)
                    Logger.LogError(ie, ie.ToString());
            }
        }

        private void TryBackup(FileWatcherInfo info)
        {
            try {
                string backupDir = Path.Combine(Const.DIRECTORY, DateTime.Now.ToString(Const.BACKUP_PATTERN));
                Directory.CreateDirectory(backupDir);

                string backupPath = Path.Combine(backupDir, info.Name);
                File.Delete(backupPath);
                File.Move(info.FullName, backupPath);
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }
        }
    }
}
