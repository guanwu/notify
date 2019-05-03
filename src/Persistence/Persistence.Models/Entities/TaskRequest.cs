using System;
using System.Collections.Generic;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class TaskRequest
    {
        public string RequestId { get; set; }
        public string Content { get; set; }
        public long CreatedAt { get; set; }

        public string TaskId { get; set; }
        public virtual JobTask Task { get; set; }
    }

    public partial class JobTask
    {
        public virtual ICollection<TaskRequest> Requests { get; set; }
    }

}
