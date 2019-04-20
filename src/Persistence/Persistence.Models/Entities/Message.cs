using System;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class Message
    {
        public string Id { get; set; }
        public string Creator { get; set; }
        public long CreationTime { get; set; }

        public string Content { get; set; }
    }
    
}
