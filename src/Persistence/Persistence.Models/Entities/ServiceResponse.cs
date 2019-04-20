using System;
using System.Collections.Generic;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class ServiceResponse
    {
        public string Id { get; set; }
        public string Creator { get; set; }
        public long CreationTime { get; set; }

        public string Content { get; set; }

        public string RequestId { get; set; }
        public virtual ServiceRequest Request { get; set; }
    }

    public partial class ServiceRequest
    {
        public virtual ICollection<ServiceResponse> Responses { get; set; }
    }

}
