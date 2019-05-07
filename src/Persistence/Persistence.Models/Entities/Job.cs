using System;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class Job
    {
        public string JobId { get; set; }
        public string Content { get; set; }
        public long CreatedAt { get; set; }
        public string SessionId { get; set; }
    }
}
