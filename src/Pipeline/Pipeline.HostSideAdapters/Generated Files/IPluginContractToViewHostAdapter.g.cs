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
    
    [System.AddIn.Pipeline.HostAdapterAttribute()]
    public class IPluginContractToViewHostAdapter : Guanwu.Notify.Views.IPlugin
    {
        private Guanwu.Notify.IPluginContract _contract;
        private System.AddIn.Pipeline.ContractHandle _handle;
        static IPluginContractToViewHostAdapter()
        {
        }
        public IPluginContractToViewHostAdapter(Guanwu.Notify.IPluginContract contract)
        {
            _contract = contract;
            _handle = new System.AddIn.Pipeline.ContractHandle(contract);
        }
        public void Initialize(Guanwu.Notify.Views.IPluginObject pluginObject)
        {
            _contract.Initialize(Guanwu.Notify.HostSideAdapters.IPluginObjectHostAdapter.ViewToContractAdapter(pluginObject));
        }
        public void Execute()
        {
            _contract.Execute();
        }
        internal Guanwu.Notify.IPluginContract GetSourceContract()
        {
            return _contract;
        }
    }
}

