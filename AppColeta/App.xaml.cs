using SOColeta.Models;

using SOTech.Core.Services;

namespace SOColeta
{
    public partial class App
    {
        public static Inventario Inventario { get; set; }
        public App()
        {
#if DEBUG
            SOTechHelper.IsDebug = true;
#endif
            InitializeComponent();
            Module.Init();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }
        protected async override void OnSleep()
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