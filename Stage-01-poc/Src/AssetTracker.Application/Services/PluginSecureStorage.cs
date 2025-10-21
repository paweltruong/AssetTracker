using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace AssetTracker.Application.Services
{
    public class PluginSecureStorage
    {
        private const string ConfigFile = "plugins.config";

        public class PluginConfig
        {
            public string PluginKey { get; set; }
            public Dictionary<string, string> Settings { get; set; } = new();
            public DateTime DateCreated { get; set; }
            public DateTime? DateModified { get; set; }
        }

        public static void SavePluginConfig(string pluginkey, Dictionary<string, string> settings)
        {
            var allConfigs = LoadAllConfigs();

            if (allConfigs.ContainsKey(pluginkey))
            {
                var pluginConfig = allConfigs[pluginkey];
                allConfigs[pluginkey] = new PluginConfig
                {
                    PluginKey = pluginkey,
                    Settings = settings,
                    DateCreated = pluginConfig.DateCreated,
                    DateModified = DateTime.Now
                };
            }
            else
            {
                allConfigs.Add(pluginkey, new PluginConfig
                {
                    PluginKey = pluginkey,
                    Settings = settings,
                    DateCreated = DateTime.Now,
                    DateModified = null
                });
            }

            SaveAllConfigs(allConfigs);
        }

        public static Dictionary<string, string> LoadPluginConfig(string pluginName)
        {
            var allConfigs = LoadAllConfigs();
            return allConfigs.TryGetValue(pluginName, out var config)
                ? config.Settings
                : new Dictionary<string, string>();
        }

        private static Dictionary<string, PluginConfig> LoadAllConfigs()
        {
            if (!File.Exists(ConfigFile))
                return new Dictionary<string, PluginConfig>();

            try
            {
                var encryptedData = File.ReadAllBytes(ConfigFile);
                var decryptedData = ProtectedData.Unprotect(
                    encryptedData, null, DataProtectionScope.CurrentUser);

                var json = Encoding.UTF8.GetString(decryptedData);
                return JsonSerializer.Deserialize<Dictionary<string, PluginConfig>>(json)
                    ?? new Dictionary<string, PluginConfig>();
            }
            catch
            {
                return new Dictionary<string, PluginConfig>();
            }
        }

        private static void SaveAllConfigs(Dictionary<string, PluginConfig> configs)
        {
            var json = JsonSerializer.Serialize(configs, new JsonSerializerOptions { WriteIndented = true });
            var encryptedData = ProtectedData.Protect(
                Encoding.UTF8.GetBytes(json), null, DataProtectionScope.CurrentUser);

            File.WriteAllBytes(ConfigFile, encryptedData);
        }
    }
}
