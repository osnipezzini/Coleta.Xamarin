using SOColeta.Models;

using System.Threading.Tasks;

namespace SOColeta.Services
{
    public interface IConfigService
    {
        Task SetConfig(Config config);
        Task<Config> GetConfigAsync();
    }
}
