//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Guanwu.Notify.Views
{
    
    public struct PipelineEvent
    {
        private string _id;
        private string _sender;
        private string _eventName;
        private System.Collections.Generic.IList<string> _eventArgs;
        public PipelineEvent(string id, string sender, string eventName, System.Collections.Generic.IList<string> eventArgs)
        {
            _id = id;
            _sender = sender;
            _eventName = eventName;
            _eventArgs = eventArgs;
        }
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public string Sender
        {
            get
            {
                return _sender;
            }
            set
            {
                _sender = value;
            }
        }
        public string EventName
        {
            get
            {
                return _eventName;
            }
            set
            {
                _eventName = value;
            }
        }
        public System.Collections.Generic.IList<string> EventArgs
        {
            get
            {
                return _eventArgs;
            }
            set
            {
                _eventArgs = value;
            }
        }
    }
}

