using System;
using System.Collections.Generic;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class JobParam
    {
        public string ParamId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string CreatedBy { get; set; }
        public long CreatedAt { get; set; }

        public string JobId { get; set; }
        public virtual Job Job { get; set; }
    }

    public partial class Job
    {
        public virtual ICollection<JobParam> Params { get; set; }
    }

}
