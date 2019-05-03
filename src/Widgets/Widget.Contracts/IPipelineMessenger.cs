using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Guanwu.Notify.Widget
{
    public delegate void PipelineMessageEventHandler(byte[] body, Dictionary<string, string> context);

    public interface IPipelineMessenger : IWidget
    {
        event PipelineMessageEventHandler OnMessageReceived;
        event PipelineMessageEventHandler OnMessagePersisting;
        event PipelineMessageEventHandler OnMessagePersisted;

        void Initialize(ILogger logger = default);
    }
}
