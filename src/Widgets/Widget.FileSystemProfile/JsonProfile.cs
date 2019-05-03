using Guanwu.Toolkit.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Guanwu.Notify.Widget.FileSystemProfile.Json
{
    [Serializable]
    public sealed class JsonProfile : IProfile
    {
        public string WidgetName => Const.PLUGIN_NAME;

        private ConcurrentDictionary<string, string> Profiles { get; set; }

        public IEnumerable<object> LoadProfile(string profileKey, string pluginName, params string[] scopes)
        {
            dynamic profile = LoadProfile(profileKey);
            if (profile == null) yield break;

            bool? isPluginActive = profile.Plugins.IsActive[pluginName];
            if (!isPluginActive.HasValue) yield break;
            if (!isPluginActive.Value) yield break;

            dynamic pluginNode = profile.Plugins[pluginName];
            if (pluginNode == null) yield break;

            foreach (dynamic nodeItem in pluginNode) {
                dynamic itemAll = nodeItem.System.All;
                if(itemAll != null) {
                    string[] conditionAll = itemAll.ToObject<string[]>();
                    if (!conditionAll.Except(scopes).Any())
                        yield return nodeItem;
                }
            }
        }

        public void Refresh()
        {
            Directory.CreateDirectory(Const.PROFILE_DIR);

            var profiles = new ConcurrentDictionary<string, string>();
            var profileFiles = Directory.EnumerateFiles(Const.PROFILE_DIR, Const.PATTERN);

            Parallel.ForEach(profileFiles, f => {
                string fileName = Path.GetFileNameWithoutExtension(f);
                string fileContent = File.ReadAllText(f);
                profiles.TryAdd(fileName, fileContent);
            });

            Profiles = profiles;
        }

        private dynamic LoadProfile(string profileKey)
        {
            string profileValue = Profiles[profileKey];
            return profileValue.FromJson<dynamic>();
        }

    }
}
