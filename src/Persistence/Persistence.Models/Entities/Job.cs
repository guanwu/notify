﻿using System;
using System.Collections.Generic;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class Job
    {
        public string JobId { get; set; }
        public string Content { get; set; }
        public long CreatedAt { get; set; }

        public string SessionId { get; set; }
        public virtual Session Session { get; set; }
    }

    public partial class Session
    {
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
