using System.Configuration;

namespace Guanwu.Notify.Plugin.Transport.Folder
{
    internal static class Const
    {
        public static readonly string TRANSPORT_DIR = ConfigurationManager.AppSettings["TRANSPORT_DIR"];
        public const string PLUGIN_NAME = "transport_folder";
    }
}
