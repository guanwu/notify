using Guanwu.Toolkit.Extensions;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace Guanwu.Notify.Widget.FileSystemProfile.Json
{
    [Serializable]
    public sealed class JsonProfile : IProfile
    {
        public string WidgetName => Const.PLUGIN_NAME;

        private ConcurrentDictionary<string, string> Profiles { get; set; }

        public dynamic Get(string name, string key = "")
        {
            string profileKey = $"{name}.{key}";
            bool existsProfile = Profiles.ContainsKey(profileKey);
            if (!existsProfile) profileKey = $"{name}.{Const.DEFAULT_KEY}";

            string profileValue = Profiles[profileKey];
            return profileValue.FromJson<dynamic>();
        }

        public void Refresh()
        {
            Directory.CreateDirectory(Const.PROFILE_DIR);

            var profiles = new ConcurrentDictionary<string, string>();
            var profileFiles = Directory.EnumerateFiles(Const.PROFILE_DIR, Const.PATTERN, SearchOption.AllDirectories);

            Parallel.ForEach(profileFiles, f => {
                string fileName = Path.GetFileNameWithoutExtension(f);
                string fileContent = File.ReadAllText(f);
                profiles.TryAdd(fileName, fileContent);
            });

            Profiles = profiles;
        }
    }
}
