using System.Collections.Generic;
using System.Linq;

namespace Guanwu.Notify.Persistence.Models
{
    public static class MessageAttributeExtension
    {
        public static string Value(this ICollection<MessageAttribute> attributes, string key, string creator)
        {
            return attributes
                .Where(t => t.Key == key && t.Creator == creator)
                .OrderByDescending(t => t.CreationTime)
                .First()
                .Value;
        }
    }
}
