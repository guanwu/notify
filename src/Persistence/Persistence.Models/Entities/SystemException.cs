using System;

namespace Guanwu.Notify.Persistence.Models
{
    [Serializable]
    public partial class SystemException
    {
        public string Id { get; set; }
        public string Creator { get; set; }
        public long CreationTime { get; set; }

        public string Message { get; set; }
        public string InnerException { get; set; }
    }
}
