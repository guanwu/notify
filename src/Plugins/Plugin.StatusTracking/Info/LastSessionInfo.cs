namespace Guanwu.Notify.Plugin.StatusTracking
{
    internal class LastSession
    {
        public string id { get; set; }
        public string sn { get; set; }
        public LastTaskState[] ls { get; set; }
        public class LastTaskState
        {
            public string n { get; set; }
            public string s { get; set; }
        }
    }

    internal class StateProfile
    {
        public string[] All { get; set; }
        public string State { get; set; }
    }
}
