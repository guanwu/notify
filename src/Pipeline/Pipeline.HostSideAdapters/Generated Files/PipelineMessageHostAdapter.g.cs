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
    
    public class PipelineMessageHostAdapter
    {
        public static Guanwu.Notify.Views.PipelineMessage ContractToViewAdapter(Guanwu.Notify.PipelineMessage contract)
        {
            return new Guanwu.Notify.Views.PipelineMessage(contract.Id, contract.Content, contract.Source, System.AddIn.Pipeline.CollectionAdapters.ToIList<string>(contract.Targets));
        }
        public static Guanwu.Notify.PipelineMessage ViewToContractAdapter(Guanwu.Notify.Views.PipelineMessage view)
        {
            return new Guanwu.Notify.PipelineMessage(view.Id, view.Content, view.Source, System.AddIn.Pipeline.CollectionAdapters.ToIListContract<string>(view.Targets));
        }
    }
}

