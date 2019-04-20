using System;
using System.AddIn.Contract;

namespace Guanwu.Notify
{
    [Serializable]
    public struct PipelineMessage
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public IListContract<string> Targets { get; set; }

        public PipelineMessage(string id, string content, string source, IListContract<string> targets)
        {
            Id = id;
            Content = content;
            Source = source;
            Targets = targets;
        }
    }
}
