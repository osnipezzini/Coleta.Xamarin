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
            Title = titulo;
        }

        public async Task OnAppearing()
        {
            await Helpers.CheckLicense();
            try
            {
                LicenseFooter = "Sistema licenciado para: " + Helpers.License.ClientName;
                if (Helpers.License.IsTimeTrial)
                    Title += " - DEMO";
            }
            catch
            {

            }
        }
    }
}
