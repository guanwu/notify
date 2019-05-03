using Guanwu.Notify.Widget;
using Guanwu.Notify.Widget.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Guanwu.Notify.Plugin.Report
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
                Logger.LogInformation($"[{Const.PLUGIN_NAME}:{nameof(BuildReport)}]<{command},{directory},{name}>");
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
            string outDir = Path.Combine(directory, report.AppId);
            outDir = Path.GetFullPath(outDir);
            Directory.CreateDirectory(outDir);

            string outPath = Path.Combine(outDir, name);
            File.WriteAllText(outPath, report.Report);
        }
    }
}
