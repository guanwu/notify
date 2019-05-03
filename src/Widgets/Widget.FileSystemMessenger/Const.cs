using System.Reflection;

namespace Guanwu.Notify.Widget.FileSystemMessenger
{
    internal class Const
    {
        public static WidgetConfig Config = new WidgetConfig(Assembly.GetExecutingAssembly().Location);
        public static readonly string BACKUP_PATTERN = Config["BACKUP_PATTERN"];
    }
}

namespace Guanwu.Notify.Widget.FileSystemMessenger.Message
{
    internal sealed class Const : FileSystemMessenger.Const
    {
        public static readonly string DIRECTORY = Config["MESSAGE_DIR"];
        public const string WIDGET_NAME = "Widget.FileSystemMessenger.Message";
        public const string FILTER = "*.json";
    }
}
