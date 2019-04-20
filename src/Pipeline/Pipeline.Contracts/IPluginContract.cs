using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Guanwu.Notify
{
    [AddInContract]
    public interface IPluginContract : IContract
    {
        void Initialize(IPluginObjectContract pluginObject);
        void Execute();
    }
}
