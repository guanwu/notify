using Guanwu.Notify.Widget;
using Guanwu.Notify.Widget.Persistence;
using Guanwu.Toolkit.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guanwu.Notify.Plugin.StatusTracking
{
    public class ReportBuilder
    {
        private readonly IRepository Repository;
        private readonly ILogger Logger;

        public ReportBuilder()
        {
            Repository = AppDomain.CurrentDomain.GetData(WidgetConst.IREPOSITORY) as IRepository;
            Logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void BuildReport(string command, string directory, string name)
        {
            try {
                var reports = QueryReports(command);
                var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
                Parallel.ForEach(reports, options, report => {
                    WriteReport(report, directory, name);
                });
                Logger.LogInformation($"[{Const.PLUGIN_NAME}:{nameof(BuildReport)}]({command})");
            }
            catch (AggregateException e) {
                foreach (var ie in e.InnerExceptions)
                    Logger.LogError(ie, ie.ToString());
            }
        }

        private ICollection<ReportInfo> QueryReports(string command)
        {
            return Repository
                .FromSql<ReportInfo>(command)
                .Where(rpt => !string.IsNullOrWhiteSpace(rpt.AppId))
                .ToArray();
        }

        private void WriteReport(ReportInfo report, string directory, string name)
        {
            string dir = Path.Combine(directory, report.AppId);
            dir = Path.GetFullPath(dir);
            Directory.CreateDirectory(dir);

            string path = Path.Combine(dir, name);

            while (File.Exists(path) && path.IsLocked()) {
                Logger.LogWarning($"File({path}) is in use, wait 5 seconds and try again.");
                SpinWait.SpinUntil(() => false, 5000);
            }

            File.WriteAllText(path, report.Report, new UTF8Encoding(false));
        }
    }
}
