using Guanwu.Notify.Persistence.Models;
using Guanwu.Notify.Views;
using Guanwu.Notify.Widget;
using Guanwu.Notify.Widget.Persistence;
using Guanwu.Toolkit.Extensions;
using Guanwu.Toolkit.Helpers;
using Guanwu.Toolkit.Utils;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Logging;
using System;
using System.AddIn;
using System.Threading.Tasks;

namespace Guanwu.Notify.Plugin.Transformer.Xml
{
    [AddIn(Const.PLUGIN_NAME)]
    public sealed class TransServer : IPlugin
    {
        private IRepository Repository;
        private IProfile Profile;
        private ILogger Logger;
        private string HangfireConn;

        public void Execute()
        {
            Guard.AgainstNull(nameof(Profile), Profile);
            Profile.Refresh();

            Guard.AgainstNullAndEmpty(nameof(HangfireConn), HangfireConn);
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSqlServerStorage(HangfireConn, new SqlServerStorageOptions {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                });
            new BackgroundJobServer(new BackgroundJobServerOptions {
                Queues = new[] { Const.PLUGIN_NAME }
            });
        }

        public void Initialize(IPluginObject pluginObject)
        {
            Repository = AppDomain.CurrentDomain.GetData(WidgetConst.IREPOSITORY) as IRepository;
            Profile = AppDomain.CurrentDomain.GetData(WidgetConst.IPROFILE) as IProfile;
            Logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;
            HangfireConn = AppDomain.CurrentDomain.GetData(WidgetConst.HANGFIRE) as string;

            pluginObject.OnMessagePersisted += OnMessagePersisted;
            Logger.LogInformation($">>>> {Const.PLUGIN_NAME} <<<<");
        }

        private void OnMessagePersisted(object sender, PipelineMessageEventArgs e)
        {
            try {
                EnqueueJobs(e.Message);
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }
        }

        private void EnqueueJobs(PipelineMessage pMessage)
        {
            string jobId = pMessage.Id;
            string appId = pMessage.Targets[WidgetConst.PMSG_APPID];
            string[] scopes = pMessage.Targets[WidgetConst.PMSG_SCOPES].Split(';');

            void EnqueueJob(dynamic profile)
            {
                string profileId = Generator.RandomLongId;
                string profileName = profile.System.ProfileName;
                string profileJson = ((object)profile).ToJson();
                Repository.AddJobParam(jobId, profileId, "Profile", profileJson, profileName, null);

                string taskId = Generator.RandomLongId;
                var taskJob = BackgroundJob.Enqueue<TransBuilder>(t => t.AddJobTask(
                    jobId, taskId, profileName));

                var stateJob = BackgroundJob.ContinueJobWith<TransBuilder>(taskJob, t => t.AddTaskState(
                    taskId, null, nameof(TaskStates.Pending)));

                string requestId = Generator.RandomLongId;
                var requestJob = BackgroundJob.ContinueJobWith<TransBuilder>(taskJob, t => t.AddTaskRequest(
                    taskId, requestId, jobId, profileId));

                BackgroundJob.ContinueJobWith<TransBuilder>(requestJob, t => t.AddTaskResponse(
                    taskId, requestId, jobId));
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
        }
    }
}
