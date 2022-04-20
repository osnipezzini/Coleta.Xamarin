using Microsoft.EntityFrameworkCore;

using SOColeta.Data;

using SOTech.Core.Services;

namespace SOColeta
{
    public partial class App
    {
        public App()
        {

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
                await licService.ValidateAsync();
        }

        protected override void OnResume()
        {
        }
    }
}