using Guanwu.Notify.Persistence.Models;
using Guanwu.Notify.Widget;
using Guanwu.Notify.Widget.Persistence;
using Guanwu.Toolkit.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Guanwu.Notify.Plugin.Transformer.Xml
{
    public class TransBuilder
    {
        private readonly IRepository Repository;
        private readonly ILogger Logger;

        public TransBuilder()
        {
            Repository = AppDomain.CurrentDomain.GetData(WidgetConst.IREPOSITORY) as IRepository;
            Logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddJobTask(string jobId, string taskId, string taskName)
        {
            Repository.AddJobTask(jobId, taskId, taskName, null);
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddTaskState(string taskId, string stateId, string stateName)
        {
            Repository.AddTaskState(taskId, stateId, stateName, null);
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddTaskRequest(string taskId, string requestId, string jobId, string profileId)
        {
            Job job = Repository.QueryEntities<Job>()
                .Single(t => t.JobId == jobId);
            JobParam profile = Repository.QueryEntities<JobParam>()
                .Single(t => t.ParamId == profileId);

            var taskRequest = new Dictionary<string, object> {
                { "Content", job.Content.FromJson<object>() },
                { "Profile", profile.Value.FromJson<dynamic>() }
            };
            string requestXml = taskRequest.ToXml();

            Repository.AddTaskRequest(taskId, requestId, requestXml, null);
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddTaskResponse(string taskId, string requestId, string jobId)
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
            Task.Run(() => TryWriteFile(directory, jobId, responseXml));

            Repository.AddTaskResponse(requestId, responseXml, null);
            AddTaskState(taskId, null, nameof(TaskStates.Completed));
        }

        private void TryWriteFile(string directory, string name, string content)
        {
            try {
                string folder = Path.Combine(directory,
                    DateTime.Now.ToString("yyyyMMdd"));
                Directory.CreateDirectory(folder);

                string path = Path.Combine(folder, $"{name}.xml");
                File.WriteAllText(path, content);
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }
        }

    }
}
