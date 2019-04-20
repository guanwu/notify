using System;
using System.AddIn.Contract;

namespace Guanwu.Notify
{
    [Serializable]
    public struct PipelineEvent
    {
        public string Id { get; set; }
        public string Sender { get; set; }
        public string EventName { get; set; }
        public IListContract<string> EventArgs { get; set; }

        public PipelineEvent(string id, string sender, string eventName, IListContract<string> eventArgs)
        {
            Id = id;
            Sender = sender;
            EventName = eventName;
            EventArgs = eventArgs;
        }
    }
}
