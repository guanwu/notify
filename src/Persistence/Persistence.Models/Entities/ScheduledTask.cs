using System;
using System.Collections.Generic;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class ScheduledTask
    {
        public string Id { get; set; }
        public string Creator { get; set; }
        public long CreationTime { get; set; }

        public long EffectiveTime { get; set; }
        public long Priority { get; set; }
        public string Executor { get; set; }
        public string Status { get; set; }

        public string MessageId { get; set; }
        public virtual Message Message { get; set; }
    }

    public partial class Message
    {
        public virtual ICollection<ScheduledTask> ScheduledTasks { get; set; }
    }

}
