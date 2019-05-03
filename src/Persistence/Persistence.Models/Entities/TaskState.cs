using System;
using System.Collections.Generic;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class TaskState
    {
        public string StateId { get; set; }
        public string StateName { get; set; }
        public long CreatedAt { get; set; }

        public string TaskId { get; set; }
        public virtual JobTask Task { get; set; }
    }

    public partial class JobTask
    {
        public virtual ICollection<TaskState> States { get; set; }
    }
}
