using Guanwu.Notify.Persistence.Models;
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

namespace Guanwu.Notify.Plugin.Transformer.Xml
{
    [Hangfire.Queue(Const.PLUGIN_NAME)]
    public class TransBuilder
    {
        private readonly IRepository Repository;
        private readonly ILogger Logger;

        public TransBuilder()
        {
            Repository = AppDomain.CurrentDomain.GetData(WidgetConst.IREPOSITORY) as IRepository;
            Logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;
        }

        public void AddJobParam(string jobId, string paramId, string name, string value, string createdBy)
        {
            Repository.AddJobParam(jobId, paramId, name, value, createdBy, null);
        }

        public void AddJobTask(string jobId, string taskId, string taskName)
        {
            Repository.AddJobTask(jobId, taskId, taskName, null);
        }

        public void AddTaskState(string taskId, string stateName)
        {
            Repository.AddTaskState(taskId, null, stateName, null);
        }

        public void AddTaskRequest(string taskId, string requestId, string jobId, string profileId)
        {
            Job job = Repository.QueryEntities<Job>()
                .Single(t => t.JobId == jobId);
            JobParam profileParam = Repository.QueryEntities<JobParam>()
                .Single(t => t.ParamId == profileId);

            var taskRequest = new Dictionary<string, object> {
                { "Content", job.Content.FromJson<object>() },
                { "Profile", profileParam.Value.FromJson<dynamic>() }
            };
            string requestXml = taskRequest.ToXml();

            Repository.AddTaskRequest(taskId, requestId, requestXml, null);
        }

        public void AddTaskResponse(string taskId, string requestId, string sessionId)
        {
            TaskRequest request = Repository.QueryEntities<TaskRequest>()
                .Single(t => t.RequestId == requestId);

            string requestXml = request.Content.FromBase64();
            dynamic taskRequest = requestXml.FromXml<dynamic>();
            dynamic profile = taskRequest.Profile.System;
            string stylesheet = profile.Stylesheet;
            string responseXml = requestXml.Transform(stylesheet);
            string directory = profile.Directory;
            string datePattern = profile.DatePattern;
            Task.Run(() => TryWriteFile(directory, sessionId, responseXml));

            Repository.AddTaskResponse(requestId, responseXml, null);
            AddTaskState(taskId, nameof(TaskStates.Completed));
        }

        private void TryWriteFile(string directory, string name, string content)
        {
            try {
                Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, $"{name}.xml");
                while (File.Exists(path) && path.IsFileLocked()) {
                    Logger.LogWarning($"File({path}) is in use, wait 5 seconds and try again.");
                    SpinWait.SpinUntil(() => false, 5000);
                }
                File.WriteAllText(path, content, new UTF8Encoding(false));
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }
        }

    }
}
