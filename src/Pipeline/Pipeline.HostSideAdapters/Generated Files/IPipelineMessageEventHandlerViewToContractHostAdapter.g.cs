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
    
    public class IPipelineMessageEventHandlerViewToContractHostAdapter : System.AddIn.Pipeline.ContractBase, Guanwu.Notify.IPipelineMessageEventHandlerContract
    {
        private object _view;
        private System.Reflection.MethodInfo _event;
        public IPipelineMessageEventHandlerViewToContractHostAdapter(object view, System.Reflection.MethodInfo eventProp)
        {
            _view = view;
            _event = eventProp;
        }
        public void EventHandler(Guanwu.Notify.IPipelineMessageEventArgsContract args)
        {
            Guanwu.Notify.Views.PipelineMessageEventArgs adaptedArgs;
            adaptedArgs = Guanwu.Notify.HostSideAdapters.PipelineMessageEventArgsHostAdapter.ContractToViewAdapter(args);
            object[] argsArray = new object[1];
            argsArray[0] = adaptedArgs;
            _event.Invoke(_view, argsArray);
        }
        internal object GetSourceView()
        {
            return _view;
        }
    }
}
