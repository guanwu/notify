//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Guanwu.Notify.HostSideAdapters
{
    
    public class IPluginObjectViewToContractHostAdapter : System.AddIn.Pipeline.ContractBase, Guanwu.Notify.IPluginObjectContract
    {
        private Guanwu.Notify.Views.IPluginObject _view;
        private System.Collections.Generic.Dictionary<Guanwu.Notify.IPipelineMessageEventHandlerContract, System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs>> OnMessageReceived_handlers;
        private System.Collections.Generic.Dictionary<Guanwu.Notify.IPipelineMessageEventHandlerContract, System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs>> OnMessagePersisting_handlers;
        private System.Collections.Generic.Dictionary<Guanwu.Notify.IPipelineMessageEventHandlerContract, System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs>> OnMessagePersisted_handlers;
        private System.Collections.Generic.Dictionary<Guanwu.Notify.IPipelineEventEventHandlerContract, System.EventHandler<Guanwu.Notify.Views.PipelineEventEventArgs>> OnEventRaising_handlers;
        private System.Collections.Generic.Dictionary<Guanwu.Notify.IPipelineEventEventHandlerContract, System.EventHandler<Guanwu.Notify.Views.PipelineEventEventArgs>> OnEventRaised_handlers;
        public IPluginObjectViewToContractHostAdapter(Guanwu.Notify.Views.IPluginObject view)
        {
            _view = view;
            OnMessageReceived_handlers = new System.Collections.Generic.Dictionary<Guanwu.Notify.IPipelineMessageEventHandlerContract, System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs>>();
            OnMessagePersisting_handlers = new System.Collections.Generic.Dictionary<Guanwu.Notify.IPipelineMessageEventHandlerContract, System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs>>();
            OnMessagePersisted_handlers = new System.Collections.Generic.Dictionary<Guanwu.Notify.IPipelineMessageEventHandlerContract, System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs>>();
            OnEventRaising_handlers = new System.Collections.Generic.Dictionary<Guanwu.Notify.IPipelineEventEventHandlerContract, System.EventHandler<Guanwu.Notify.Views.PipelineEventEventArgs>>();
            OnEventRaised_handlers = new System.Collections.Generic.Dictionary<Guanwu.Notify.IPipelineEventEventHandlerContract, System.EventHandler<Guanwu.Notify.Views.PipelineEventEventArgs>>();
        }
        public virtual void MessageReceivedEventAdd(Guanwu.Notify.IPipelineMessageEventHandlerContract handler)
        {
            System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs> adaptedHandler = new System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs>(new Guanwu.Notify.HostSideAdapters.IPipelineMessageEventHandlerContractToViewHostAdapter(handler).Handler);
            _view.OnMessageReceived += adaptedHandler;
            OnMessageReceived_handlers[handler] = adaptedHandler;
        }
        public virtual void MessageReceivedEventRemove(Guanwu.Notify.IPipelineMessageEventHandlerContract handler)
        {
            System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs> adaptedHandler;
            if (OnMessageReceived_handlers.TryGetValue(handler, out adaptedHandler))
            {
                OnMessageReceived_handlers.Remove(handler);
                _view.OnMessageReceived -= adaptedHandler;
            }
        }
        public virtual void MessagePersistingEventAdd(Guanwu.Notify.IPipelineMessageEventHandlerContract handler)
        {
            System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs> adaptedHandler = new System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs>(new Guanwu.Notify.HostSideAdapters.IPipelineMessageEventHandlerContractToViewHostAdapter(handler).Handler);
            _view.OnMessagePersisting += adaptedHandler;
            OnMessagePersisting_handlers[handler] = adaptedHandler;
        }
        public virtual void MessagePersistingEventRemove(Guanwu.Notify.IPipelineMessageEventHandlerContract handler)
        {
            System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs> adaptedHandler;
            if (OnMessagePersisting_handlers.TryGetValue(handler, out adaptedHandler))
            {
                OnMessagePersisting_handlers.Remove(handler);
                _view.OnMessagePersisting -= adaptedHandler;
            }
        }
        public virtual void MessagePersistedEventAdd(Guanwu.Notify.IPipelineMessageEventHandlerContract handler)
        {
            System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs> adaptedHandler = new System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs>(new Guanwu.Notify.HostSideAdapters.IPipelineMessageEventHandlerContractToViewHostAdapter(handler).Handler);
            _view.OnMessagePersisted += adaptedHandler;
            OnMessagePersisted_handlers[handler] = adaptedHandler;
        }
        public virtual void MessagePersistedEventRemove(Guanwu.Notify.IPipelineMessageEventHandlerContract handler)
        {
            System.EventHandler<Guanwu.Notify.Views.PipelineMessageEventArgs> adaptedHandler;
            if (OnMessagePersisted_handlers.TryGetValue(handler, out adaptedHandler))
            {
                OnMessagePersisted_handlers.Remove(handler);
                _view.OnMessagePersisted -= adaptedHandler;
            }
        }
        public virtual void EventRaisingEventAdd(Guanwu.Notify.IPipelineEventEventHandlerContract handler)
        {
            System.EventHandler<Guanwu.Notify.Views.PipelineEventEventArgs> adaptedHandler = new System.EventHandler<Guanwu.Notify.Views.PipelineEventEventArgs>(new Guanwu.Notify.HostSideAdapters.IPipelineEventEventHandlerContractToViewHostAdapter(handler).Handler);
            _view.OnEventRaising += adaptedHandler;
            OnEventRaising_handlers[handler] = adaptedHandler;
        }
        public virtual void EventRaisingEventRemove(Guanwu.Notify.IPipelineEventEventHandlerContract handler)
        {
            System.EventHandler<Guanwu.Notify.Views.PipelineEventEventArgs> adaptedHandler;
            if (OnEventRaising_handlers.TryGetValue(handler, out adaptedHandler))
            {
                OnEventRaising_handlers.Remove(handler);
                _view.OnEventRaising -= adaptedHandler;
            }
        }
        public virtual void EventRaisedEventAdd(Guanwu.Notify.IPipelineEventEventHandlerContract handler)
        {
            System.EventHandler<Guanwu.Notify.Views.PipelineEventEventArgs> adaptedHandler = new System.EventHandler<Guanwu.Notify.Views.PipelineEventEventArgs>(new Guanwu.Notify.HostSideAdapters.IPipelineEventEventHandlerContractToViewHostAdapter(handler).Handler);
            _view.OnEventRaised += adaptedHandler;
            OnEventRaised_handlers[handler] = adaptedHandler;
        }
        public virtual void EventRaisedEventRemove(Guanwu.Notify.IPipelineEventEventHandlerContract handler)
        {
            System.EventHandler<Guanwu.Notify.Views.PipelineEventEventArgs> adaptedHandler;
            if (OnEventRaised_handlers.TryGetValue(handler, out adaptedHandler))
            {
                OnEventRaised_handlers.Remove(handler);
                _view.OnEventRaised -= adaptedHandler;
            }
        }
        public virtual void ReceiveMessage(Guanwu.Notify.PipelineMessage pMessage)
        {
            _view.ReceiveMessage(Guanwu.Notify.HostSideAdapters.PipelineMessageHostAdapter.ContractToViewAdapter(pMessage));
        }
        public virtual void PersistingMessage(Guanwu.Notify.PipelineMessage pMessage)
        {
            _view.PersistingMessage(Guanwu.Notify.HostSideAdapters.PipelineMessageHostAdapter.ContractToViewAdapter(pMessage));
        }
        public virtual void PersistedMessage(Guanwu.Notify.PipelineMessage pMessage)
        {
            _view.PersistedMessage(Guanwu.Notify.HostSideAdapters.PipelineMessageHostAdapter.ContractToViewAdapter(pMessage));
        }
        public virtual void RaisingEvent(Guanwu.Notify.PipelineEvent pEvent)
        {
            _view.RaisingEvent(Guanwu.Notify.HostSideAdapters.PipelineEventHostAdapter.ContractToViewAdapter(pEvent));
        }
        public virtual void RaisedEvent(Guanwu.Notify.PipelineEvent pEvent)
        {
            _view.RaisedEvent(Guanwu.Notify.HostSideAdapters.PipelineEventHostAdapter.ContractToViewAdapter(pEvent));
        }
        internal Guanwu.Notify.Views.IPluginObject GetSourceView()
        {
            return _view;
        }
    }
}

