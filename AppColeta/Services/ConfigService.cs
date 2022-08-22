using SOColeta.Models;

using SOCore.Utils;

using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SOColeta.Services
{
    public class ConfigService : IConfigService
    {
        private Config config;
        private readonly string configFile = Path.Combine(SOHelper.AppDataFolder, $"{SOHelper.AppName}.cfg");
        public ConfigService()
        {
            config = Load() ?? new Config();           
        }

        private Config? Load()
        {
            if (File.Exists(configFile))
            {
                try
                {
                    var jsonConfig = File.ReadAllText(configFile);
                    var config = JsonSerializer.Deserialize<Config>(jsonConfig);
                    return config;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public Task<Config> GetConfigAsync()
        {
            return Task.FromResult(Load() ?? new Config());
        }

        public Task SetConfig(Config config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            try
            {
                var jsonConfig = JsonSerializer.Serialize(config);
                File.WriteAllText(configFile, jsonConfig);
            }
            catch (Exception)
            {

            }

            return Task.FromResult(config);
        }
    }
}
