using System.IO;
using System.Text.Json;

namespace TaroVS.Services
{
    public class ConfigService
    {
        private const string ConfigFileName = "appconfig.json";

        public AppConfig LoadConfig()
        {
            if (!File.Exists(ConfigFileName))
            {
                var defaultConfig = new AppConfig();
                SaveConfig(defaultConfig);
                return defaultConfig;
            }

            var json = File.ReadAllText(ConfigFileName);
            var config = JsonSerializer.Deserialize<AppConfig>(json);

            return config ?? new AppConfig();
        }

        public void SaveConfig(AppConfig config)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(ConfigFileName, json);
        }
    }
}