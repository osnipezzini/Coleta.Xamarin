using Microsoft.EntityFrameworkCore;

using SOColeta.Data;

using SOCore.Services;

using System;

namespace SOColeta
{
    public partial class App
    {
        public App()
        {
#if DEBUG
            Environment.SetEnvironmentVariable("SOTECHDEV", "1");
#endif
            InitializeComponent();
            Module.Init();

            using (var context = new AppDbContext())
                context.Database.Migrate();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }
        protected override async void OnSleep()
        {
            var licService = Module.GetService<ILicenseService>();
            if (licService != null && licService.HasLicense)
                await licService.ValidateDeviceAsync();
        }

        protected override void OnResume()
        {
        }
    }
}