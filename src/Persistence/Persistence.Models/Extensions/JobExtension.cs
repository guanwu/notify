using System.Collections.Generic;
using System.Linq;

namespace Guanwu.Notify.Persistence.Models
{
    public static class JobExtension
    {
        public static string Last(this ICollection<JobParam> jobParams, string createdBy, string paramName)
        {
            return jobParams
                .Where(t => t.CreatedBy == createdBy && t.Name == paramName)
                .OrderBy(t => t.CreatedAt)
                .LastOrDefault()?.Value;
        }

        public static string Last(this ICollection<TaskState> taskStates)
        {
            return taskStates
                .OrderBy(t => t.CreatedAt)
                .LastOrDefault()?.StateName;
        }
        public static string Last(this ICollection<SessionState> sessionStates)
        {
            return sessionStates
                .OrderBy(t => t.CreatedAt)
                .LastOrDefault()?.StateName;
        }
    }
}
