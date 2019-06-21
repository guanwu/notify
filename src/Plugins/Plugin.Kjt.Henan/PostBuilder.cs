using Guanwu.Notify.Persistence.Models;
using Guanwu.Notify.Widget;
using Guanwu.Notify.Widget.Persistence;
using Guanwu.Toolkit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Guanwu.Notify.Plugin.Kjt.Henan
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
        public void AddTaskRequest(string taskId, string requestId, string jobId)
        {
            Job job = Repository.QueryEntities<Job>()
                .Single(t => t.JobId == jobId);

            string data = job.Content.FromJson<dynamic>().data;
            string xmlMsg = data.FromBase64();

            dynamic entity = xmlMsg.FromXml<dynamic>();
            dynamic payment = entity["ceb:Payment"];
            dynamic paymentHead = payment["ceb:PaymentHead"];
            string ebpCode = paymentHead["ceb:ebpCode"];
            string guid = paymentHead["ceb:guid"];
            string orderNo = paymentHead["ceb:orderNo"];
            string payerIdNumber = paymentHead["ceb:payerIdNumber"];
            string amountPaid = paymentHead["ceb:amountPaid"];
            string payCode = paymentHead["ceb:payCode"];
            string payName = paymentHead["ceb:payName"];
            string payTime = paymentHead["ceb:payTime"];
            string payTransactionId = paymentHead["ceb:payTransactionId"];
            string payerName = paymentHead["ceb:payerName"];
            string payTimeStr = payTime.Insert(12, ":").Insert(10, ":")
                .Insert(8, " ").Insert(6, "-").Insert(4, "-");

            var taskRequest = new Dictionary<string, string> {
                { "_input_charset", "utf-8" },
                { "eCommerceCode", ebpCode },
                { "freight", "0" },
                { "notify_id", guid },
                { "notify_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "notify_type", "forex_customs_info" },
                { "orderNo", orderNo },
                { "payAccount", payerIdNumber },
                { "payAmount", amountPaid },
                { "payAmountCurr", "142" },
                { "payEnterpriseCode", payCode },
                { "payEnterpriseName", payName },
                { "payGoodsAmount", "0" },
                { "payMerchantCode", ebpCode },
                { "payTaxAmount", "0" },
                { "payTimeStr", payTimeStr },
                { "payTransactionNo", payTransactionId },
                { "payerCertNo", payerIdNumber },
                { "payerCertType", "1" },
                { "payerName", payerName },
                { "xmlMsg", xmlMsg }
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
            string responseText = "";
            using (var httpClient = new HttpClient()) {
                var httpContent = new FormUrlEncodedContent(requestForms);
                var httpResponse = httpClient.PostAsync(requestHost, httpContent).Result;
                httpResponse.EnsureSuccessStatusCode();
                responseText = httpResponse.Content.ReadAsStringAsync().Result;
            }
            Repository.AddTaskResponse(requestId, responseText, null);

            string stateName = responseText == "success" ?
                nameof(TaskStates.Completed) : nameof(TaskStates.Failed);
            AddTaskState(taskId, stateName);
        }
    }
}