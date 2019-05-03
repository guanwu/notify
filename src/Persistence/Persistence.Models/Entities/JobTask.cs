using System;
using System.Collections.Generic;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class JobTask
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public long CreatedAt { get; set; }

        public string JobId { get; set; }
        public virtual Job Job { get; set; }
    }

    public partial class Job
    {
        public virtual ICollection<JobTask> Tasks { get; set; }
    }

}
