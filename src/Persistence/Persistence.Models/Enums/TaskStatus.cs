using System;

namespace Guanwu.Notify.Persistence.Models
{
    [Flags]
    public enum TaskStatus
    {
        Pending = 0x10,
        Processing = 0x20,

        Hold = 0x100,
        Fault = 0x200,

        Failed = 0x1000,
        Completed = 0x2000,
        Cancelled = 0x4000,
    }
}
