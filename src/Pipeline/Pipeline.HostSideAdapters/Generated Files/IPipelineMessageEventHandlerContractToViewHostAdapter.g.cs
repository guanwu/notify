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
    
    public class IPipelineMessageEventHandlerContractToViewHostAdapter
    {
        private Guanwu.Notify.IPipelineMessageEventHandlerContract _contract;
        private System.AddIn.Pipeline.ContractHandle _handle;
        public IPipelineMessageEventHandlerContractToViewHostAdapter(Guanwu.Notify.IPipelineMessageEventHandlerContract contract)
        {
            _contract = contract;
            _handle = new System.AddIn.Pipeline.ContractHandle(contract);
        }
        public void Handler(object sender, Guanwu.Notify.Views.PipelineMessageEventArgs args)
        {
            _contract.EventHandler(Guanwu.Notify.HostSideAdapters.PipelineMessageEventArgsHostAdapter.ViewToContractAdapter(args));
        }
        internal Guanwu.Notify.IPipelineMessageEventHandlerContract GetSourceContract()
        {
            return _contract;
        }
    }
}

