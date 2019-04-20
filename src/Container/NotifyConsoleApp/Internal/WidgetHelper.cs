using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Guanwu.NotifyConsoleApp
{
    internal static class WidgetHelper
    {
        public static ICollection<Type> FromWidgetTypes()
        {
            // Load all top-level widget directories
            var directories = Directory
                .GetDirectories(ContainerConst.WIDGET_PATH, "*", SearchOption.TopDirectoryOnly)
                .Select(t => Path.GetFullPath(t));

            // Load types of namesake-assembly
            var types = directories
                .Select(t => new { File = Path.Combine(t, Path.GetFileName(t) + ".dll") })
                .Where(t => File.Exists(t.File))
                .SelectMany(t => Assembly.LoadFrom(t.File).ExportedTypes);

            return types.ToArray();
        }
    }
}
