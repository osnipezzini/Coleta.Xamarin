using SOTech.Core.Services;

namespace SOColeta
{
    public partial class App
    {
        public App()
        {
#if DEBUG
            SOTech.Core.SOTechHelper.IsDebug = true;
#endif
            InitializeComponent();
            Module.Init();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }
        protected override async void OnSleep()
        {
            var licService = Module.GetService<ILicenseService>();
            if (licService != null && licService.HasLicense)
                await licService.ValidateAsync();
        }

        protected override void OnResume()
        {
        }
    }
}