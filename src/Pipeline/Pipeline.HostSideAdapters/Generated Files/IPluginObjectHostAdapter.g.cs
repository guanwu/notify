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
    
    public class IPluginObjectHostAdapter
    {
        internal static Guanwu.Notify.Views.IPluginObject ContractToViewAdapter(Guanwu.Notify.IPluginObjectContract contract)
        {
            if ((contract == null))
            {
                return null;
            }
            if (((System.Runtime.Remoting.RemotingServices.IsObjectOutOfAppDomain(contract) != true) 
                        && contract.GetType().Equals(typeof(IPluginObjectViewToContractHostAdapter))))
            {
                return ((IPluginObjectViewToContractHostAdapter)(contract)).GetSourceView();
            }
            else
            {
                return new IPluginObjectContractToViewHostAdapter(contract);
            }
        }
        internal static Guanwu.Notify.IPluginObjectContract ViewToContractAdapter(Guanwu.Notify.Views.IPluginObject view)
        {
            if ((view == null))
            {
                return null;
            }
            if (view.GetType().Equals(typeof(IPluginObjectContractToViewHostAdapter)))
            {
                return ((IPluginObjectContractToViewHostAdapter)(view)).GetSourceContract();
            }
            else
            {
                return new IPluginObjectViewToContractHostAdapter(view);
            }
        }
    }
}
