using System;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class TaskResponse
    {
        public string RequestId { get; set; }
        public string Content { get; set; }
        public long CreatedAt { get; set; }

        public virtual TaskRequest Request { get; set; }
    }

    public partial class TaskRequest
    {
        public virtual TaskResponse Response { get; set; }
    }

}
