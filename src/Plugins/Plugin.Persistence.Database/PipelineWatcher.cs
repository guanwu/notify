using Guanwu.Notify.Views;
using Guanwu.Notify.Widget;
using Guanwu.Notify.Widget.Persistence;
using Guanwu.Toolkit.Extensions;
using Guanwu.Toolkit.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.AddIn;

namespace Guanwu.Notify.Plugin.Persistence.Database
{
    [AddIn(Const.PLUGIN_NAME)]
    public sealed class PipelineWatcher : IPlugin
    {
        private string PluginName => Const.PLUGIN_NAME;

        private IPluginObject PluginObject;
        private IRepository Repository;
        private ILogger Logger;

        public void Execute() { }

        public void Initialize(IPluginObject pluginObject)
        {
            Repository = AppDomain.CurrentDomain.GetData(WidgetConst.IREPOSITORY) as IRepository;
            Logger = AppDomain.CurrentDomain.GetData(WidgetConst.ILOGGER) as ILogger;

            PluginObject = pluginObject;
            PluginObject.OnMessageReceived += OnMessageReceived;

            Logger.LogInformation($">>>> {PluginName} <<<<");
        }

        private void OnMessageReceived(object sender, PipelineMessageEventArgs e)
        {
            try {
                PipelineMessage message = e.Message;
                ExtractMessageTargets(ref message);
                PluginObject.PersistingMessage(message);
                PersistMessage(message);
                PluginObject.PersistedMessage(message);
            }
            catch (Exception ex) {
                Logger.LogError(ex, ex.ToString());
            }
        }

        private void ExtractMessageTargets(ref PipelineMessage pMessage)
        {
            string content = pMessage.Content;
            Guard.AgainstNullAndEmpty(nameof(content), content);
            dynamic entity = content.FromJson<dynamic>();
            Guard.AgainstNull(nameof(entity), entity);
            dynamic auth = entity.auth;
            Guard.AgainstNull(nameof(auth), auth);
            string appId = auth.app_id;
            Guard.AgainstNullAndEmpty(nameof(appId), appId);
            dynamic meta = entity.meta;
            Guard.AgainstNull(nameof(meta), meta);
            string scopes = meta.scopes;
            Guard.AgainstNullAndEmpty(nameof(scopes), scopes);

            pMessage.Targets = new[] { appId, scopes };
        }

        private void PersistMessage(PipelineMessage pMessage)
        {
            Guard.AgainstNullAndEmpty(nameof(pMessage.Content), pMessage.Content);
            Guard.AgainstNullAndEmpty(nameof(pMessage.Source), pMessage.Source);
            Guard.AgainstNullAndEmpty(nameof(pMessage.Id), pMessage.Id);

            try {
                Repository.AddJob(pMessage.Id, pMessage.Content, null);
            }
            catch (Exception ex) {
                Logger.LogCritical(ex, ex.ToString());
            }
        }

        //private void SaveMessageAttributes(ref PipelineMessage pMessage)
        //{
        //    Guard.AgainstNullAndEmpty(nameof(pMessage.Content), pMessage.Content);
        //    Guard.AgainstNullAndEmpty(nameof(pMessage.Source), pMessage.Source);
        //    Guard.AgainstNullAndEmpty(nameof(pMessage.Id), pMessage.Id);

        //    string messageId = pMessage.Id;
        //    long timestamp = Generator.Timestamp;
        //    MessageAttribute BuildAttribute(string name, string value)
        //    {
        //        return new MessageAttribute {
        //            CreationTime = timestamp,
        //            Creator = PluginName,
        //            Id = Generator.RandomLongId,
        //            MessageId = messageId,
        //            Key = name,
        //            Value = value,
        //        };
        //    }

        //    var attributes = new List<MessageAttribute>();
        //    attributes.Add(BuildAttribute(WidgetConst.PMSG_APPID, pMessage.Targets[0]));
        //    attributes.AddRange(pMessage.Targets[1].Split(';').Select(t => BuildAttribute(WidgetConst.PMSG_SCOPE, t)));

        //    int rows = DbContext.InsertEntities(attributes);
        //    if (rows > 0)
        //        Logger.LogInformation($"{rows} rows affected.");
        //    else
        //        Logger.LogCritical($"Message({messageId}) attributes persistence exception.");
        //}
    }
}
