using Microsoft.EntityFrameworkCore;

using SOColeta.Data;

using SOFramework.Services;

using System;
using System.Diagnostics;

namespace SOColeta
{
    public partial class App
    {
        private IUILicenseService licenseService;
        public App()
        {
            InitializeDebug();
            InitializeComponent();
            Module.Init();

            using (var context = new AppDbContext())
                context.Database.Migrate();

            licenseService = Module.GetService<IUILicenseService>();

            MainPage = new AppShell();
        }
        [Conditional("DEBUG")]
        private void InitializeDebug()
        {
            Environment.SetEnvironmentVariable("SOLOGLEVEL", "10");
            Environment.SetEnvironmentVariable("SODEBUG", "1");
            Environment.SetEnvironmentVariable("SOTECHHML", "1");
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