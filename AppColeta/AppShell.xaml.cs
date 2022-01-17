using SOColeta.Views;

using SOTech.Core.Services;

using Xamarin.Forms;

namespace SOColeta
{
    public partial class AppShell
    {
        private readonly ILicenseService licenseService;
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CriarColetaPage), typeof(CriarColetaPage));
            Routing.RegisterRoute(nameof(ConfigPage), typeof(ConfigPage));
            licenseService = Module.GetService<ILicenseService>();
        }

        protected override async void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            if (args.Current.Location.OriginalString == "///LicensePage")
                return;

            if (!licenseService.HasLicense)
                await GoToAsync("///LicensePage");
        }
    }
}
