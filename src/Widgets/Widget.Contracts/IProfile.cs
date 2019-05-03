using System.Collections.Generic;

namespace Guanwu.Notify.Widget
{
    public interface IProfile : IWidget
    {
        void Refresh();
        IEnumerable<object> LoadProfile(string profileKey, string pluginName, params string[] scopes);
    }
}
