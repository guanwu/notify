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
    
    public class PipelineEventEventArgsContractToViewHostAdapter : Guanwu.Notify.Views.PipelineEventEventArgs
    {
        private Guanwu.Notify.IPipelineEventEventArgsContract _contract;
        private System.AddIn.Pipeline.ContractHandle _handle;
        static PipelineEventEventArgsContractToViewHostAdapter()
        {
        }
        public PipelineEventEventArgsContractToViewHostAdapter(Guanwu.Notify.IPipelineEventEventArgsContract contract)
        {
            _contract = contract;
            _handle = new System.AddIn.Pipeline.ContractHandle(contract);
        }
        public override Guanwu.Notify.Views.PipelineEvent Event
        {
            get
            {
                return Guanwu.Notify.HostSideAdapters.PipelineEventHostAdapter.ContractToViewAdapter(_contract.Event);
            }
            set
            {
                _contract.Event = Guanwu.Notify.HostSideAdapters.PipelineEventHostAdapter.ViewToContractAdapter(value);
            }
        }
        internal Guanwu.Notify.IPipelineEventEventArgsContract GetSourceContract()
        {
            return _contract;
        }
    }
}

