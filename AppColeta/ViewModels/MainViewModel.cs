using SOTech.Core.Services;
using SOTech.Mvvm;

using System.Threading.Tasks;

namespace SOColeta.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private string license_footer;
        private readonly ILicenseService licenseService;

        public string LicenseFooter { get => license_footer; private set => SetProperty(ref license_footer, value); }
        public MainViewModel(ILicenseService licenseService)
        {
            string titulo = "SOColeta";
            Title = titulo;
            this.licenseService = licenseService;
        }

        public override async Task OnAppearing()
        {            
            try
            {
                LicenseFooter = "Sistema licenciado para: " + LicenseService.Instance.License.ClientName;
            }
            catch
            {

            }

            await Task.CompletedTask;
        }
    }
}
