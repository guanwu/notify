using Guanwu.Notify.Persistence.Models;
using Guanwu.Notify.Widget;
using Guanwu.Notify.Widget.Persistence;
using Guanwu.Toolkit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Guanwu.Notify.Plugin.Kjt.Alipay
{
    public class PostBuilder
    {
        private readonly IRepository Repository;

        public PostBuilder()
        {
            Repository = AppDomain.CurrentDomain.GetData(WidgetConst.IREPOSITORY) as IRepository;
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddJobParam(string jobId, string paramId, string name, string value, string createdBy)
        {
            Repository.AddJobParam(jobId, paramId, name, value, createdBy, null);
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddJobTask(string jobId, string taskId, string taskName)
        {
            Repository.AddJobTask(jobId, taskId, taskName, null);
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddTaskState(string taskId, string stateName)
        {
            Repository.AddTaskState(taskId, null, stateName, null);
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddTaskRequest(string taskId, string requestId, string jobId, string profileId)
        {
            Job job = Repository.QueryEntities<Job>()
                .Single(t => t.JobId == jobId);
            JobParam profileParam = Repository.QueryEntities<JobParam>()
                .Single(t => t.ParamId == profileId);

            dynamic profile = profileParam.Value.FromJson<dynamic>();
            string signKey = profile.User.SignKey;
            string requestData = job.Content.FromJson<dynamic>().data;
            string requestXml = requestData.FromBase64();

            var requestItems = requestXml
                .FromXml<SortedDictionary<string, string>>();
            var signKeys = requestItems.Single(t => t.Key == "@signKeys").Value.Split(';');
            var signItems = requestItems
                .Where(t => !string.IsNullOrEmpty(t.Value))
                .Where(t => signKeys.Contains(t.Key));

            string requestText = string.Join("&", signItems.Select(t => $"{t.Key}={t.Value}"));
            string requestSign = $"{requestText}{signKey}".ToMd5().ToLower();
            string requestPath = $"{requestText}&sign={requestSign}&sign_type=MD5";

            Repository.AddTaskRequest(taskId, requestId, requestPath, null);
            AddTaskState(taskId, nameof(TaskStates.Processing));
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddTaskResponse(string taskId, string requestId, string requestHost)
        {
            TaskRequest taskRequest = Repository.QueryEntities<TaskRequest>()
                .Single(t => t.RequestId == requestId);

            string requestPath = taskRequest.Content.FromBase64();
            string requestUrl = $"{requestHost}?{requestPath}";
            string responseXml = "";
            using (var httpClient = new HttpClient()) {
                var httpResponse = httpClient.GetAsync(requestUrl).Result;
                httpResponse.EnsureSuccessStatusCode();
                string responseString = httpResponse.Content.ReadAsStringAsync().Result;
                responseXml = Uri.UnescapeDataString(responseString);
            }
            Repository.AddTaskResponse(requestId, responseXml, null);

            dynamic response = responseXml.FromXml<dynamic>();
            string stateName = (response.is_success == "T"
                && (response.response.alipay.result_code == "SUCCESS" || response.response.alipay.detail_error_code == "SAME_CUSTOMS_DECLARE_ONCE")) ?
                nameof(TaskStates.Completed) : nameof(TaskStates.Failed);
            AddTaskState(taskId, stateName);
        }
    }
}