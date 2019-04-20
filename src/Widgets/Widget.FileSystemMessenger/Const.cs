using System.Reflection;

namespace Guanwu.Notify.Widget.FileSystemMessenger
{
    internal class Const
    {
        public static WidgetConfig Config = new WidgetConfig(Assembly.GetExecutingAssembly().Location);
        public static readonly string MESSAGE_DIR = Config["MESSAGE_DIR"];
        public static readonly string BACKUP_PATTERN = Config["BACKUP_PATTERN"];
    }
}

namespace Guanwu.Notify.Widget.FileSystemMessenger.Json
{
    internal sealed class Const : FileSystemMessenger.Const
    {
        public const string WIDGET_NAME = "Widget.FileSystemMessage.JsonMessage";
        public const string FILTER = "*.json";
    }
}
