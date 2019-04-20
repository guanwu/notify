using System;
using System.Configuration;
using System.Reflection;

namespace Guanwu.Notify.Widget
{
    public sealed class WidgetConfig
    {
        private static readonly Lazy<WidgetConfig> lazy =
            new Lazy<WidgetConfig>(() => new WidgetConfig((string)AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE")));
        public static WidgetConfig Widget { get { return lazy.Value; } }

        private KeyValueConfigurationCollection appSettings;

        public WidgetConfig(string exePath)
        {
            if (exePath == null)
                exePath = Assembly.GetExecutingAssembly().Location;

            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
            appSettings = config.AppSettings.Settings;
        }

        public string this[string key]
        {
            get
            {
                var config = appSettings[key];
                return config == null ? null : config.Value;
            }
        }
    }
}
