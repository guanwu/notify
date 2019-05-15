using System;
using System.Collections.Generic;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class SessionState
    {
        public string StateId { get; set; }
        public string StateName { get; set; }
        public long CreatedAt { get; set; }

        public string SessionId { get; set; }
        public virtual Session Session { get; set; }
    }

    public partial class Session
    {
        public virtual ICollection<SessionState> States { get; set; }
    }
}
