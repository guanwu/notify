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
    
    public class PipelineEventArrayHostAdapter
    {
        public static Guanwu.Notify.Views.PipelineEvent[] ContractToViewAdapter(Guanwu.Notify.PipelineEvent[] contract)
        {
            if ((contract == null))
            {
                return null;
            }
            Guanwu.Notify.Views.PipelineEvent[] result = new Guanwu.Notify.Views.PipelineEvent[contract.Length];
            for (int i = 0; (i < contract.Length); i = (i + 1))
            {
                result[i] = Guanwu.Notify.HostSideAdapters.PipelineEventHostAdapter.ContractToViewAdapter(contract[i]);
            }
            return result;
        }
        public static Guanwu.Notify.PipelineEvent[] ViewToContractAdapter(Guanwu.Notify.Views.PipelineEvent[] view)
        {
            if ((view == null))
            {
                return null;
            }
            Guanwu.Notify.PipelineEvent[] result = new Guanwu.Notify.PipelineEvent[view.Length];
            for (int i = 0; (i < view.Length); i = (i + 1))
            {
                result[i] = Guanwu.Notify.HostSideAdapters.PipelineEventHostAdapter.ViewToContractAdapter(view[i]);
            }
            return result;
        }
    }
}

