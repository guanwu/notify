using System;
using System.Collections.Generic;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class ServiceRequest
    {
        public string Id { get; set; }
        public string Creator { get; set; }
        public long CreationTime { get; set; }

        public string Content { get; set; }

        public string TaskId { get; set; }
        public virtual ScheduledTask Task { get; set; }
    }

    public partial class ScheduledTask
    {
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
    }

}
