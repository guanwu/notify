using System.Configuration;

namespace Guanwu.NotifyConsoleApp
{
    internal static class ContainerConst
    {
        public const string WIDGET_PATH = "Widgets";
        public const string PLUGIN_PATH = "Plugins";
        public const string PLUGIN_LOCATION = "PLUGIN_LOCATION";
        public static readonly string WIDGET_PRIVATE_PATH = ConfigurationManager.AppSettings["WIDGET_PRIVATE_PATH"];
    }
}
