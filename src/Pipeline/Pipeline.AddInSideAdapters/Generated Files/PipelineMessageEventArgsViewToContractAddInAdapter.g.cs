//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Guanwu.Notify.AddInSideAdapters
{
    
    public class PipelineMessageEventArgsViewToContractAddInAdapter : System.AddIn.Pipeline.ContractBase, Guanwu.Notify.IPipelineMessageEventArgsContract
    {
        private Guanwu.Notify.Views.PipelineMessageEventArgs _view;
        public PipelineMessageEventArgsViewToContractAddInAdapter(Guanwu.Notify.Views.PipelineMessageEventArgs view)
        {
            _view = view;
        }
        public Guanwu.Notify.PipelineMessage Message
        {
            get
            {
                return Guanwu.Notify.AddInSideAdapters.PipelineMessageAddInAdapter.ViewToContractAdapter(_view.Message);
            }
            set
            {
                _view.Message = Guanwu.Notify.AddInSideAdapters.PipelineMessageAddInAdapter.ContractToViewAdapter(value);
            }
        }
        internal Guanwu.Notify.Views.PipelineMessageEventArgs GetSourceView()
        {
            return _view;
        }
    }
}

