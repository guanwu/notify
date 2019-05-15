using System;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class Session
    {
        public string SessionId { get; set; }
        public string AppId { get; set; }
        public long CreatedAt { get; set; }
    }
}
