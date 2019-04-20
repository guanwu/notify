using Guanwu.Notify.Persistence.Models;
using Guanwu.Notify.Views;
using Guanwu.Notify.Widget;
using Guanwu.Toolkit.Helpers;
using Guanwu.Toolkit.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.AddIn;

namespace Guanwu.Notify.Plugin.Persistence.Database
{
    [AddIn(Const.PLUGIN_NAME)]
    public sealed class PipelineWatcher : IPlugin
    {
        private IPluginObject pluginObject;
        private ILogger logger;
        private IDbContext dbContext;

        private string pluginName => Const.PLUGIN_NAME;

        public void Execute() { }

        public void Initialize(IPluginObject pluginObject)
        {
            logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;
            logger.LogInformation($"[PLUGIN_INITIALIZED] {pluginName}");

            dbContext = AppDomain.CurrentDomain.GetData(WidgetConst.IDBCONTEXT) as IDbContext;

            this.pluginObject = pluginObject;
            this.pluginObject.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, PipelineMessageEventArgs e)
        {
            try
            {
                PipelineMessage message = e.Message;
                pluginObject.PersistingMessage(message);
                SavePipelineMessage(ref message);
                pluginObject.PersistedMessage(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        private void SavePipelineMessage(ref PipelineMessage pMessage)
        {
            Guard.AgainstNullAndEmpty(nameof(pMessage.Content), pMessage.Content);
            Guard.AgainstNullAndEmpty(nameof(pMessage.Source), pMessage.Source);
            Guard.AgainstNullAndEmpty(nameof(pMessage.Id), pMessage.Id);

            var message = new Message()
            {
                Content = pMessage.Content,
                CreationTime = Generator.Timestamp,
                Creator = pMessage.Source,
                Id = pMessage.Id,
            };

            if (dbContext.InsertEntities(message) == 0)
                logger.LogError($"Data persistence failed and needs to be processed immediately.", message);
        }
    }
}
