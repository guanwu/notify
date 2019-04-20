using System;
using System.Collections.Generic;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class MessageAttribute
    {
        public string Id { get; set; }
        public string Creator { get; set; }
        public long CreationTime { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public string MessageId { get; set; }
        public virtual Message Message { get; set; }
    }

    public partial class Message
    {
        public virtual ICollection<MessageAttribute> Attributes { get; set; }
    }

}
