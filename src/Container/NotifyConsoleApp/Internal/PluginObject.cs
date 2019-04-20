
using Guanwu.Notify.Views;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Guanwu.NotifyConsoleApp
{
    [Serializable]
    internal sealed class PluginObject : IPluginObject
    {
        public event EventHandler<PipelineMessageEventArgs> OnMessageReceived;
        public event EventHandler<PipelineMessageEventArgs> OnMessagePersisting;
        public event EventHandler<PipelineMessageEventArgs> OnMessagePersisted;
        public event EventHandler<PipelineEventEventArgs> OnEventRaising;
        public event EventHandler<PipelineEventEventArgs> OnEventRaised;

        private readonly ILogger logger;

        public PluginObject(ILogger logger)
        {
            this.logger = logger;
        }

        public void ReceiveMessage(PipelineMessage pMessage)
        {
            if (OnMessageReceived == null) return;
            Parallel.ForEach(OnMessageReceived.GetInvocationList(), t =>
            {
                try
                {
                    ((EventHandler<PipelineMessageEventArgs>)t).BeginInvoke(
                        this, new PipelineMessageEventArgs() { Message = pMessage }, null, null);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, ex.Message);
                }
            });
        }

        public void PersistingMessage(PipelineMessage pMessage)
        {
            if (OnMessagePersisting == null) return;
            Parallel.ForEach(OnMessagePersisting.GetInvocationList(), t =>
            {
                try
                {
                    ((EventHandler<PipelineMessageEventArgs>)t).BeginInvoke(
                        this, new PipelineMessageEventArgs() { Message = pMessage }, null, null);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, ex.Message);
                }
            });
        }

        public void PersistedMessage(PipelineMessage pMessage)
        {
            if (OnMessagePersisted == null) return;
            Parallel.ForEach(OnMessagePersisted.GetInvocationList(), t =>
            {
                try
                {
                    ((EventHandler<PipelineMessageEventArgs>)t).BeginInvoke(
                        this, new PipelineMessageEventArgs() { Message = pMessage }, null, null);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, ex.Message);
                }
            });
        }

        public void RaisingEvent(PipelineEvent pEvent)
        {
            if (OnEventRaising == null) return;
            Parallel.ForEach(OnEventRaising.GetInvocationList(), t =>
            {
                try
                {
                    ((EventHandler<PipelineEventEventArgs>)t).BeginInvoke(
                        this, new PipelineEventEventArgs() { Event = pEvent }, null, null);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, ex.Message);
                }
            });
        }

        public void RaisedEvent(PipelineEvent pEvent)
        {
            if (OnEventRaised == null) return;
            Parallel.ForEach(OnEventRaised.GetInvocationList(), t =>
            {
                try
                {
                    ((EventHandler<PipelineEventEventArgs>)t).BeginInvoke(
                        this, new PipelineEventEventArgs() { Event = pEvent }, null, null);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, ex.Message);
                }
            });
        }
    }
}
