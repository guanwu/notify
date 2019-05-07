using Guanwu.Notify.Persistence.Models;
using Guanwu.Toolkit.Extensions;
using Guanwu.Toolkit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guanwu.Notify.Widget.Persistence
{
    public class Repository : IRepository
    {
        public string WidgetName => Const.WIDGET_NAME;
        private IDbContext DbContext => AppDomain.CurrentDomain.GetData(WidgetConst.IDBCONTEXT) as IDbContext;

        public void AddJob(string sessionId, string jobId, string content, long? createdAt)
        {
            var job = new Job {
                Content = content,
                CreatedAt = createdAt ?? Generator.Timestamp,
                JobId = jobId ?? Generator.RandomLongId,
                SessionId = sessionId ?? Generator.RandomLongId,
            };
            DbContext?.InsertEntities(job);
        }

        public void AddJobParam(string jobId, string paramId, string name, string value, string createdBy, long? createdAt)
        {
            var param = new JobParam {
                CreatedAt = createdAt ?? Generator.Timestamp,
                CreatedBy = createdBy,
                JobId = jobId,
                Name = name,
                ParamId = paramId ?? Generator.RandomLongId,
                Value = value,
            };
            DbContext?.InsertEntities(param);
        }

        public void AddJobTask(string jobId, string taskId, string taskName, long? createdAt)
        {
            var task = new JobTask {
                CreatedAt = createdAt ?? Generator.Timestamp,
                JobId = jobId,
                TaskId = taskId ?? Generator.RandomLongId,
                TaskName = taskName,
            };
            DbContext?.InsertEntities(task);
        }

        public void AddTaskRequest(string taskId, string requestId, string content, long? createdAt)
        {
            var request = new TaskRequest {
                Content = content?.ToBase64(),
                CreatedAt = createdAt ?? Generator.Timestamp,
                RequestId = requestId ?? Generator.RandomLongId,
                TaskId = taskId,
            };
            DbContext?.InsertEntities(request);
        }

        public void AddTaskResponse(string requestId, string content, long? createdAt)
        {
            var response = new TaskResponse {
                Content = content?.ToBase64(),
                CreatedAt = createdAt ?? Generator.Timestamp,
                RequestId = requestId,
            };
            DbContext?.InsertEntities(response);
        }

        public void AddTaskState(string taskId, string stateId, string stateName, long? createdAt)
        {
            var state = new TaskState {
                CreatedAt = createdAt ?? Generator.Timestamp,
                StateId = stateId ?? Generator.RandomLongId,
                StateName = stateName,
                TaskId = taskId,
            };
            DbContext?.InsertEntities(state);
        }

        public List<TElement> FromSql<TElement>(string sql, params object[] parameters)
        {
            return DbContext?.FromSql<TElement>(sql, parameters);
        }

        public IQueryable<TEntity> QueryEntities<TEntity>() where TEntity : class
        {
            return DbContext?.QueryEntities<TEntity>();
        }

    }
}
