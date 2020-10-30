using SOTechLib.Utils;
using System.Threading.Tasks;

namespace AppColeta.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private string license_footer;
        public string LicenseFooter { get => license_footer; private set => SetProperty(ref license_footer, value); }
        public MainViewModel()
        {
            string titulo = "AppColeta";
            if (Helper.TKT)
                titulo += " - TKT";
            if (Helper.Debug)
                titulo += " - Debug";
            Title = titulo;
        }

        public async Task OnAppearing()
        {
            await Helpers.CheckLicense();
            try
            {
                LicenseFooter = "Sistema licenciado para: " + Helpers.License.ClientName;
            }
            catch
            {

            }
        }
    }
}
