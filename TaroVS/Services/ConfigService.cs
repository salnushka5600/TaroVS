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
                var config = new AppConfig();
                SaveConfig(config);
                return config;
            }

            var json = File.ReadAllText(ConfigFileName);
            return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
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