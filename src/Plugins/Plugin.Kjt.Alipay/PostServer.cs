using Guanwu.Notify.Persistence.Models;
using Guanwu.Notify.Views;
using Guanwu.Notify.Widget;
using Guanwu.Toolkit.Extensions;
using Guanwu.Toolkit.Helpers;
using Guanwu.Toolkit.Utils;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Logging;
using System;
using System.AddIn;
using System.Threading.Tasks;

namespace Guanwu.Notify.Plugin.Kjt.Alipay
{
    [AddIn(Const.PLUGIN_NAME)]
    public sealed class PostServer : IPlugin
    {
        private IProfile Profile;
        private ILogger Logger;
        private string HangfireConn;

        public void Execute()
        {
            try {
                Guard.AgainstNull(nameof(Profile), Profile);
                Profile.Refresh();

                Guard.AgainstNullAndEmpty(nameof(HangfireConn), HangfireConn);
                GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSqlServerStorage(HangfireConn, new SqlServerStorageOptions {
                    PrepareSchemaIfNecessary = false,
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true,
                });
                new BackgroundJobServer(new BackgroundJobServerOptions {
                    Queues = new[] { Const.PLUGIN_NAME },
                });
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }
        }

        public void Initialize(IPluginObject pluginObject)
        {
            Profile = AppDomain.CurrentDomain.GetData(WidgetConst.IPROFILE) as IProfile;
            Logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;
            HangfireConn = AppDomain.CurrentDomain.GetData(WidgetConst.HANGFIRE) as string;

            pluginObject.OnMessagePersisted += OnMessagePersisted;
            //Logger.LogInformation($">>>> {Const.PLUGIN_NAME}: {AppDomain.CurrentDomain.Id} <<<<");
        }

        private void OnMessagePersisted(object sender, PipelineMessageEventArgs e)
        {
            if (e == null) return;

            Task.Run(() => TryEnqueueJobs(e.Message));
        }

        private void TryEnqueueJobs(PipelineMessage pMessage)
        {
            string jobId = pMessage.Targets[WidgetConst.PMSGIDX_JOBID];
            string appId = pMessage.Targets[WidgetConst.PMSGIDX_APPID];
            string[] scopes = pMessage.Targets[WidgetConst.PMSGIDX_SCOPES].Split(';');

            void EnqueueJob(dynamic profile)
            {
                string profileId = Generator.RandomLongId;
                string profileName = profile.System.ProfileName;
                string requestHost = profile.System.RequestHost;
                string profileJson = ((object)profile).ToJson();
                string paramJob = BackgroundJob.Enqueue<PostBuilder>(t => t.AddJobParam(
                    jobId, profileId, "Profile", profileJson, profileName));

                string taskId = Generator.RandomLongId;
                var taskJob = BackgroundJob.ContinueJobWith<PostBuilder>(paramJob, t => t.AddJobTask(
                    jobId, taskId, profileName));

                var stateJob = BackgroundJob.ContinueJobWith<PostBuilder>(taskJob, t => t.AddTaskState(
                    taskId, nameof(TaskStates.Pending)));

                string requestId = Generator.RandomLongId;
                var requestJob = BackgroundJob.ContinueJobWith<PostBuilder>(taskJob, t => t.AddTaskRequest(
                    taskId, requestId, jobId, profileId));

                BackgroundJob.ContinueJobWith<PostBuilder>(requestJob, t => t.AddTaskResponse(
                    taskId, requestId, requestHost));
            }

            try {
                var profiles = Profile.LoadProfile(appId, Const.PLUGIN_NAME, scopes);
                Parallel.ForEach(profiles, profile => {
                    EnqueueJob(profile);
                });
            }
            catch (AggregateException e) {
                foreach (var ie in e.InnerExceptions)
                    Logger.LogError(ie, ie.ToString());
            }
            catch (Exception e) {
                Logger.LogError(e, e.ToString());
            }
        }
    }
}
