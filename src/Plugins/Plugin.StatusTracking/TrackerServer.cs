using Guanwu.Notify.Persistence.Models;
using Guanwu.Notify.Views;
using Guanwu.Notify.Widget;
using Guanwu.Toolkit.Extensions;
using Guanwu.Toolkit.Scheduling;
using Guanwu.Toolkit.Utils;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Logging;
using System;
using System.AddIn;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Guanwu.Notify.Plugin.StatusTracking
{
    [AddIn(Const.PLUGIN_NAME)]
    public sealed class TrackerServer : IPlugin
    {
        private IProfile Profile;
        private ILogger Logger;
        private string HangfireConn;

        private readonly List<string> SchedulerJobs = new List<string>();

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

                var scheduler = new Scheduler { CronExpression = "*/2 * * * *" };
                scheduler.OnTime += (s, e) => {
                    BackgroundJob.Enqueue<ReportBuilder>(t => t.BuildReport("EXEC [sp_last_session]", "C:\\Saturn\\v4\\web_ui\\publish\\Data", "last_status.json"));
                };
                Task.Run(() => scheduler.Start());
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
            string sessionId = pMessage.Id;
            string appId = pMessage.Targets[WidgetConst.PMSGIDX_APPID];
            string[] scopes = pMessage.Targets[WidgetConst.PMSGIDX_SCOPES].Split(';');

            void EnqueueJob(dynamic profile)
            {
                string profileJson = ((object)(profile.User)).ToJson();
                string profileName = profile.System.ProfileName;
                string procedure = profile.System.Procedure;
                string cronExpression = profile.System.CronExpression;
                int maxAge = profile.System.MaxAge;

                BackgroundJob.Enqueue<TrackerBuilder>(t => t.AddSessionState(
                sessionId, nameof(SessionStates.Pending)));

                string syncJob = $"{appId}.{profileName}";
                if (!SchedulerJobs.Contains(syncJob)) {
                    var scheduler = new Scheduler { CronExpression = cronExpression };
                    scheduler.OnTime += (s, e) => {
                        BackgroundJob.Enqueue<TrackerBuilder>(t => t.SyncSessionStates(procedure, profileJson));
                    };
                    SchedulerJobs.Add(syncJob);
                    Task.Run(() => { scheduler.Start(); scheduler.Dispose(); });
                    Task.Run(() => { SpinWait.SpinUntil(() => false, maxAge); scheduler.Abort(); SchedulerJobs.Remove(syncJob); });
                }
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
