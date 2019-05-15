﻿using Guanwu.Notify.Views;
using Guanwu.Notify.Widget;
using Guanwu.Toolkit.Utils;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Logging;
using System;
using System.AddIn;

namespace Guanwu.Notify.Plugin.Report
{
    [AddIn(Const.PLUGIN_NAME)]
    public sealed class ReportServer : IPlugin
    {
        private ILogger Logger;
        private string HangfireConn;

        public void Execute()
        {
            try {
                Guard.AgainstNullAndEmpty(nameof(HangfireConn), HangfireConn);
                GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSqlServerStorage(HangfireConn, new SqlServerStorageOptions {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true,
                });
                new BackgroundJobServer(new BackgroundJobServerOptions {
                    ServerName = $"{Environment.MachineName}.{Const.PLUGIN_NAME}",
                    Queues = new[] { Const.PLUGIN_NAME },
                });
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }

            RecurringJob.AddOrUpdate<ReportBuilder>("report.last_session", t => t.BuildReport(
                "EXEC [sp_last_session];", "Output\\Kjt.Reports", "last_status.json"), 
                Cron.Minutely, timeZone: TimeZoneInfo.Local, queue: Const.PLUGIN_NAME);
        }

        public void Initialize(IPluginObject pluginObject)
        {
            Logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;
            HangfireConn = AppDomain.CurrentDomain.GetData(WidgetConst.HANGFIRE) as string;

            Logger.LogInformation($">>>> {Const.PLUGIN_NAME}: {AppDomain.CurrentDomain.Id} <<<<");
        }
    }
}
