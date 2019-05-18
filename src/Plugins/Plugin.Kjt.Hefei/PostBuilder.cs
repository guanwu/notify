using Guanwu.Notify.Persistence.Models;
using Guanwu.Notify.Widget;
using Guanwu.Notify.Widget.Persistence;
using Guanwu.Toolkit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Guanwu.Notify.Plugin.Kjt.Hefei
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
            string userId = profile.User.UserId;
            string userToken = profile.User.Token;
            string dxpId = profile.User.DxpId;
            string msgType = profile.System.MsgType;
            string declType = profile.System.DeclType;
            string copMsgId = Guid.NewGuid().ToString();
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msg = job.Content.FromJson<dynamic>().data;
            string sign = $"copMsgId={copMsgId}&decltype={declType}" +
                $"&dxpId={dxpId}&msg={msg}&msgtype={msgType}&timestamp={timestamp}" +
                $"&userid={userId}&key={userToken}".ToMd5();
            
            var taskRequest = new Dictionary<string, string> {
                { "userid", userId },
                { "timestamp", timestamp },
                { "sign", sign },
                { "decltype", declType },
                { "msg", msg },
                { "msgtype", msgType },
                { "dxpId", dxpId },
                { "copMsgId", copMsgId },
            };
            string requestJson = taskRequest.ToJson();

            Repository.AddTaskRequest(taskId, requestId, requestJson, null);
            AddTaskState(taskId, nameof(TaskStates.Processing));
        }

        [Hangfire.Queue(Const.PLUGIN_NAME)]
        public void AddTaskResponse(string taskId, string requestId, string requestHost)
        {
            TaskRequest taskRequest = Repository.QueryEntities<TaskRequest>()
                .Single(t => t.RequestId == requestId);

            string requestJson = taskRequest.Content.FromBase64();
            var requestForms = requestJson.FromJson<Dictionary<string, string>>();
            string responseXml = "";
            using (var httpClient = new HttpClient()) {
                var httpContent = new FormUrlEncodedContent(requestForms);
                var httpResponse = httpClient.PostAsync(requestHost, httpContent).Result;
                httpResponse.EnsureSuccessStatusCode();
                string responseString = httpResponse.Content.ReadAsStringAsync().Result;
                responseXml = Uri.UnescapeDataString(responseString);
            }
            Repository.AddTaskResponse(requestId, responseXml, null);

            dynamic response = responseXml.FromXml<dynamic>();
            string responseResult = response.Header.Result;
            string responseMessage = response.Header.ResultMsg;
            string stateName = (responseResult == "T" || responseMessage == "REPEATED_CALL_ERROR!") ?
                nameof(TaskStates.Completed) : nameof(TaskStates.Failed);

            AddTaskState(taskId, stateName);
        }
    }
}
