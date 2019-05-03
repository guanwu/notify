using System.Reflection;

namespace Guanwu.Notify.Widget.FileSystemProfile
{
    internal class Const
    {
        public static WidgetConfig Config = new WidgetConfig(Assembly.GetExecutingAssembly().Location);
        public static readonly string PROFILE_DIR = Config["PROFILE_DIR"];
    }
}

namespace Guanwu.Notify.Widget.FileSystemProfile.Json
{
    internal sealed class Const : FileSystemProfile.Const
    {
        public const string PLUGIN_NAME = "Widget.FileSystemProfile.JsonProfile";
        public const string PATTERN = "*.json";
    }
}
