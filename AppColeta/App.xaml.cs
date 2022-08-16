using Microsoft.EntityFrameworkCore;

using SOColeta.Data;

using SOFramework.Services;

namespace SOColeta
{
    public partial class App
    {
        private IUILicenseService licenseService;
        public App()
        {

            InitializeComponent();
            Module.Init();

            using (var context = new AppDbContext())
                context.Database.Migrate();

            licenseService = Module.GetService<IUILicenseService>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            licenseService.ValidateOnStart();
        }
        protected override void OnSleep()
        {
            licenseService.ValidateOnSleep();
        }

        protected override void OnResume()
        {
        }
    }
}