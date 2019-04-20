using System.AddIn.Contract;

namespace Guanwu.Notify
{
    public interface IPluginObjectContract : IContract
    {
        void MessageReceivedEventAdd(IPipelineMessageEventHandlerContract handler);
        void MessageReceivedEventRemove(IPipelineMessageEventHandlerContract handler);
        
        void MessagePersistingEventAdd(IPipelineMessageEventHandlerContract handler);
        void MessagePersistingEventRemove(IPipelineMessageEventHandlerContract handler);
        
        void MessagePersistedEventAdd(IPipelineMessageEventHandlerContract handler);
        void MessagePersistedEventRemove(IPipelineMessageEventHandlerContract handler);
        
        void EventRaisingEventAdd(IPipelineEventEventHandlerContract handler);
        void EventRaisingEventRemove(IPipelineEventEventHandlerContract handler);
        
        void EventRaisedEventAdd(IPipelineEventEventHandlerContract handler);
        void EventRaisedEventRemove(IPipelineEventEventHandlerContract handler);

        void ReceiveMessage(PipelineMessage pMessage);
        void PersistingMessage(PipelineMessage pMessage);
        void PersistedMessage(PipelineMessage pMessage);
        void RaisingEvent(PipelineEvent pEvent);
        void RaisedEvent(PipelineEvent pEvent);
    }

    public interface IPipelineMessageEventHandlerContract : IContract
    {
        void EventHandler(IPipelineMessageEventArgsContract args);
    }

    public interface IPipelineMessageEventArgsContract : IContract
    {
        PipelineMessage Message { get; set; }
    }

    public interface IPipelineEventEventHandlerContract : IContract
    {
        void EventHandler(IPipelineEventEventArgsContract args);
    }

    public interface IPipelineEventEventArgsContract : IContract
    {
        PipelineEvent Event { get; set; }
    }
}
