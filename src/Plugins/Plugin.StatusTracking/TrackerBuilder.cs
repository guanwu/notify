using Guanwu.Notify.Widget;
using Guanwu.Notify.Widget.Persistence;
using Guanwu.Toolkit.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guanwu.Notify.Plugin.StatusTracking
{
    public class TrackerBuilder
    {
        private readonly IRepository Repository;
        private readonly ILogger Logger;

        public TrackerBuilder()
        {
            Repository = AppDomain.CurrentDomain.GetData(WidgetConst.IREPOSITORY) as IRepository;
            Logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddSessionState(string sessionId, string stateName)
        {
            Repository.AddSessionState(sessionId, null, stateName, null);
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void SyncSessionStates(string command, string profileJson)
        {
            try {
                var reports = QueryReports(command);
                var profiles = profileJson.FromJson<StateProfile[]>();
                var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
                Parallel.ForEach(reports, options, report => {
                    SyncSessionState(report.Report, profiles);
                });
                Logger.LogInformation($"[{Const.PLUGIN_NAME}:{nameof(SyncSessionStates)}]({command})");
            }
            catch (AggregateException e) {
                foreach (var ie in e.InnerExceptions)
                    Logger.LogError(ie, ie.ToString());
            }
        }

        private void SyncSessionState(string reportJson, StateProfile[] profiles)
        {
            var lastSessions = reportJson.FromJson<LastSession[]>();
            foreach (var session in lastSessions) {
                string currentState = GetSessionState(session.ls, profiles);
                if (currentState != null && currentState != session.sn)
                    AddSessionState(session.id, currentState);
            }
        }

        private string GetSessionState(LastSession.LastTaskState[] taskStates, StateProfile[] profiles)
        {
            var scopes = taskStates.Select(t => $"{t.n}:{t.s}");
            foreach (var profile in profiles) {
                var conditionAll = profile.All;
                if (!conditionAll.Except(scopes).Any())
                    return profile.State;
            }
            return null;
        }

        private ICollection<ReportInfo> QueryReports(string command)
        {
            return Repository
                .FromSql<ReportInfo>(command)
                .ToArray();
        }
    }
}
