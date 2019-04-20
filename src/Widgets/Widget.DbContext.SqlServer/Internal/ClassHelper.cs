using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Guanwu.Notify.Widget.DbContext.SqlServer
{
    internal static class ClassHelper
    {
        public static ICollection<Assembly> FromAssemblies()
        {
            // Load all top-level model config directories
            var directories = Directory
                .GetDirectories(Const.PERSISTENCE, "*", SearchOption.TopDirectoryOnly)
                .Select(t => Path.GetFullPath(t));

            // Load namesake-assembly
            var assemblies = directories
                .Select(t => new { File = Path.Combine(t, Path.GetFileName(t) + ".dll") })
                .Where(t => File.Exists(t.File))
                .Select(t => Assembly.LoadFrom(t.File));

            return assemblies.ToArray();
        }
    }
}
