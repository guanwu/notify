using System.AddIn.Contract;
using System.AddIn.Pipeline;

[assembly: PipelineHints.ShareViews]
[assembly: PipelineHints.SegmentAssemblyName(PipelineHints.PipelineSegment.Views, "Pipeline.ShareViews")]
[assembly: PipelineHints.SegmentAssemblyName(PipelineHints.PipelineSegment.HostSideAdapter, "Pipeline.HostSideAdapters")]
[assembly: PipelineHints.SegmentAssemblyName(PipelineHints.PipelineSegment.AddInSideAdapter, "Pipeline.AddInSideAdapters")]

namespace Guanwu.Notify
{
    [AddInContract]
    public interface IPluginContract : IContract
    {
        void Initialize(IPluginObjectContract pluginObject);
        void Execute();
    }
    
    public interface IPluginObjectContract : IContract
    {
        [PipelineHints.EventAdd("OnMessageReceived")]
        void MessageReceivedEventAdd(IPipelineMessageEventHandlerContract handler);
        [PipelineHints.EventRemove("OnMessageReceived")]
        void MessageReceivedEventRemove(IPipelineMessageEventHandlerContract handler);
        
        [PipelineHints.EventAdd("OnMessagePersisting")]
        void MessagePersistingEventAdd(IPipelineMessageEventHandlerContract handler);
        [PipelineHints.EventRemove("OnMessagePersisting")]
        void MessagePersistingEventRemove(IPipelineMessageEventHandlerContract handler);
        
        [PipelineHints.EventAdd("OnMessagePersisted")]
        void MessagePersistedEventAdd(IPipelineMessageEventHandlerContract handler);
        [PipelineHints.EventRemove("OnMessagePersisted")]
        void MessagePersistedEventRemove(IPipelineMessageEventHandlerContract handler);
        
        [PipelineHints.EventAdd("OnEventRaising")]
        void EventRaisingEventAdd(IPipelineEventEventHandlerContract handler);
        [PipelineHints.EventRemove("OnEventRaising")]
        void EventRaisingEventRemove(IPipelineEventEventHandlerContract handler);

        [PipelineHints.EventAdd("OnEventRaised")]
        void EventRaisedEventAdd(IPipelineEventEventHandlerContract handler);
        [PipelineHints.EventRemove("OnEventRaised")]
        void EventRaisedEventRemove(IPipelineEventEventHandlerContract handler);
        
        void ReceiveMessage(PipelineMessage pMessage);
        void PersistingMessage(PipelineMessage pMessage);
        void PersistedMessage(PipelineMessage pMessage);
        void RaisingEvent(PipelineEvent pEvent);
        void RaisedEvent(PipelineEvent pEvent);
    }

    [PipelineHints.EventHandler]
    public interface IPipelineMessageEventHandlerContract : IContract
    {
        void EventHandler(IPipelineMessageEventArgsContract args);
    }

    [PipelineHints.EventArgs]
    public interface IPipelineMessageEventArgsContract : IContract
    {
        PipelineMessage Message { get; set; }
    }
    
    [PipelineHints.EventHandler]
    public interface IPipelineEventEventHandlerContract : IContract
    {
        void EventHandler(IPipelineEventEventArgsContract args);
    }

    [PipelineHints.EventArgs]
    public interface IPipelineEventEventArgsContract : IContract
    {
        PipelineEvent Event { get; set; }
    }

}
