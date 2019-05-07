using System.Collections.Generic;
using System.Linq;

namespace Guanwu.Notify.Widget.Persistence
{
    public interface IRepository : IWidget
    {
        List<TElement> FromSql<TElement>(string sql, params object[] parameters);
        IQueryable<TEntity> QueryEntities<TEntity>() where TEntity : class;

        void AddJob(
            string sessionId, 
            string jobId, 
            string content, 
            long? createdAt);

        void AddJobParam(
            string jobId,
            string paramId,
            string name,
            string value,
            string createdBy,
            long? createdAt);

        void AddJobTask(
            string jobId,
            string taskId,
            string taskName,
            long? createdAt);

        void AddTaskRequest(
           string taskId,
           string requestId,
           string content,
           long? createdAt);

        void AddTaskResponse(
            string requestId,
            string content,
            long? createdAt);

        void AddTaskState(
            string taskId,
            string stateId,
            string stateName,
            long? createdAt);
    }
}
