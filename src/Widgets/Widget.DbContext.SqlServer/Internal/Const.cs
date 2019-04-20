using System.Reflection;

namespace Guanwu.Notify.Widget.DbContext.SqlServer
{
    internal static class Const
    {
        public static readonly string PERSISTENCE = @"D:\DevOps\Guanwu\Guanwu.Notify\Bin\Persistence";
        private static WidgetConfig widgetConfig = new WidgetConfig(Assembly.GetExecutingAssembly().Location);

        public const string WIDGET_NAME = "Widget.DbContext.SqlServer";
        //public static readonly string PERSISTENCE = widgetConfig["Persistence"];
        public static readonly string DBCONNECT = widgetConfig["DbConnect"];
    }
}
