using Guanwu.Notify.Views;
using Guanwu.Notify.Widget;
using Microsoft.Extensions.Logging;
using System;
using System.AddIn;

namespace Guanwu.Notify.Plugin.Persistence.Console
{
    [AddIn(Const.PLUGIN_NAME)]
    public sealed class PipelineWatcher : IPlugin
    {
        private ILogger logger;

        private string pluginName => Const.PLUGIN_NAME;

        public void Execute() { }

        public void Initialize(IPluginObject pluginObject)
        {
            logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;
            logger.LogInformation($"[PLUGIN_INITIALIZED] {pluginName}");

            pluginObject.OnMessageReceived += OnMessageReceived;
            pluginObject.OnEventRaising += OnEventRaising;
        }

        private void OnEventRaising(object sender, PipelineEventEventArgs e)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("MM-dd HH:mm:ss");
                string eventSender = e.Event.Sender;
                string eventName = e.Event.EventName;
                string eventArgs = string.Join(Environment.NewLine, e.Event.EventArgs);

                logger.LogInformation(
                    $"[EVENT_RAISING][{timestamp}][{eventSender}]\r\n" +
                    $"[EVENT]:{eventName}\r\n" +
                    $"[EVENT ARGS]:{eventArgs}\r\n"
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        private void OnMessageReceived(object sender, PipelineMessageEventArgs e)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("MM-dd HH:mm:ss");
                string source = e.Message.Source;
                string content = e.Message.Content;

                logger.LogInformation(
                    $"[MESSAGE_RECEIVED][{timestamp}][{source}]\r\n" +
                    $"[CONTENT]:{content}\r\n"
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
