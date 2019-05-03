using System.Reflection;

namespace Guanwu.Notify.Widget.DbContext.SqlServer
{
    internal static class Const
    {
        private static WidgetConfig Config = new WidgetConfig(Assembly.GetExecutingAssembly().Location);

        public const string WIDGET_NAME = "Widget.DbContext.SqlServer";
        public static readonly string PERSISTENCE = Config["Persistence"];
        public static readonly string DBCONNECT = Config["DbConnect"];
    }
}
