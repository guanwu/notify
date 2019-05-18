using System;

namespace Guanwu.Notify.Plugin.StatusTracking
{
    [Serializable]
    internal sealed class ReportInfo
    {
        public string AppId { get; set; }
        public string Report { get; set; }
    }
}
