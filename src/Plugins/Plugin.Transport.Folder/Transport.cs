using Guanwu.Notify.Views;
using Guanwu.Notify.Widget;
using Guanwu.Toolkit.IO;
using Guanwu.Toolkit.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.AddIn;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guanwu.Notify.Plugin.Transport.Folder
{
    [AddIn(Const.PLUGIN_NAME)]
    public sealed class Transport : IPlugin
    {
        private IProfile Profile;
        private ILogger Logger;
        private FileWatcher FileWatcher;

        public void Execute()
        {
            try {
                Guard.AgainstNull(nameof(Profile), Profile);
                Profile.Refresh();

                Directory.CreateDirectory(Const.TRANSPORT_DIR);
                FileWatcher = new FileWatcher {
                    Recursive = true,
                    Directory = Const.TRANSPORT_DIR
                };
                FileWatcher.OnFileCreated += OnFileCreated;
                Task.Run(() => FileWatcher.Start());
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }
        }

        public void Initialize(IPluginObject pluginObject)
        {
            Profile = AppDomain.CurrentDomain.GetData(WidgetConst.IPROFILE) as IProfile;
            Logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;

            Logger.LogInformation($">>>> {Const.PLUGIN_NAME}: {AppDomain.CurrentDomain.Id} <<<<");
        }

        private void OnFileCreated(object sender, FileWatcherInfo e)
        {
            if (e == null) return;

            Task.Run(() => TryDelete(e));
            Task.Run(() => TryWriteJobMessage(e));
        }

        private void TryWriteJobMessage(FileWatcherInfo info)
        {
            void WriteJobMessage(dynamic profile, string appId)
            {
                string jobId = Path.GetFileNameWithoutExtension(info.Name);
                string data = Convert.ToBase64String(info.FileBytes);
                string directory = profile.System.Directory;
                string scopes = profile.System.Scopes;

                string jobMessage =
                    "{\"auth\":{\"app_id\":\"" +
                    appId +
                    "\"},\"meta\":{\"id\":\"" +
                    jobId +
                    "\",\"scopes\":\"" +
                    scopes +
                    "\"},\"data\":\"" +
                    data
                    + "\"}";

                string messageDir = profile.System.Directory;
                string path = Path.Combine(directory, jobId + ".json");

                while (File.Exists(path)) {
                    Logger.LogWarning("File already exists, wait 5 seconds and try again.");
                    SpinWait.SpinUntil(() => false, 5000);
                }

                File.WriteAllText(path, jobMessage, new UTF8Encoding(false));
            }

            try {
                string[] folders = info.DirectoryName.Split('\\');
                if (folders.Count() < 3) throw new ArgumentException(info.DirectoryName);

                string[] reverseFolders = folders.Reverse().ToArray();
                string scopes = reverseFolders[0].ToLower();
                string appId = reverseFolders[1].ToLower();
                Guard.AgainstNullAndEmpty(nameof(scopes), scopes);
                Guard.AgainstNullAndEmpty(nameof(appId), appId);

                var profiles = Profile.LoadProfile(appId, Const.PLUGIN_NAME, scopes);
                Parallel.ForEach(profiles, profile => {
                    WriteJobMessage(profile, appId);
                });
            }
            catch (AggregateException e) {
                foreach (var ie in e.InnerExceptions)
                    Logger.LogError(ie, ie.ToString());
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }
        }

        private void TryDelete(FileWatcherInfo info)
        {
            try {
                File.Delete(info.FullName);
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }
        }

    }
}
