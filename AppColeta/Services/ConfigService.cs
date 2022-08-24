using SOColeta.Models;

using SOCore.Utils;

using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;

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

        private Config Load()
        {
            if (File.Exists(configFile))
            {
                try
                {
                    var jsonConfig = File.ReadAllText(configFile);
                    config = JsonSerializer.Deserialize<Config>(jsonConfig);
                    Module.SwipeMode = config.SwipeMode;
                    return config;
                }
                catch (Exception)
                {
                    return new Config();
                }
            }
            return new Config();
        }

        public Task<Config> GetConfigAsync()
        {
            return Task.FromResult(config ?? Load() ?? new Config());
        }

        public void SetSwipeMode(SwipeMode swipeMode)
        {
            config.SwipeMode = swipeMode;
            SetConfig(config);
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
