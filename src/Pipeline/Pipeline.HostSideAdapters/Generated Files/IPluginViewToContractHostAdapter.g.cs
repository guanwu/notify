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
    
    public class IPluginViewToContractHostAdapter : System.AddIn.Pipeline.ContractBase, Guanwu.Notify.IPluginContract
    {
        private Guanwu.Notify.Views.IPlugin _view;
        public IPluginViewToContractHostAdapter(Guanwu.Notify.Views.IPlugin view)
        {
            _view = view;
        }
        public virtual void Initialize(Guanwu.Notify.IPluginObjectContract pluginObject)
        {
            _view.Initialize(Guanwu.Notify.HostSideAdapters.IPluginObjectHostAdapter.ContractToViewAdapter(pluginObject));
        }
        public virtual void Execute()
        {
            _view.Execute();
        }
        internal Guanwu.Notify.Views.IPlugin GetSourceView()
        {
            return _view;
        }
    }
}

