using SOColeta.Models;
using SOColeta.Services;

using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    class ConfigViewModel : ViewModelBase
    {
        private readonly IConfigService configService;
        public Config Config { get; private set; }
        public ConfigViewModel(IConfigService configService)
        {
            Title = "Configurações";
            this.configService = configService;
        }
        public override async Task OnAppearing()
        {
            Config = await configService.GetConfigAsync();
        }
        public void SetSwipeMode(SwipeMode swipeMode)
        {
            Config.SwipeMode = swipeMode;
            configService.SetConfig(Config);
        }
    }
}
