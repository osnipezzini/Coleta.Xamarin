using Microsoft.EntityFrameworkCore;

using SOColeta.Data;
using SOColeta.Services;

using SOCore.Services;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta
{
    public partial class App
    {
        public App(IStockService stockService)
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

            Device.StartTimer(new TimeSpan(0, 5, 0), () =>
            {
                Task.Factory.StartNew(async () =>
                {
                    var stockService = Module.GetService<IStockService>();
                    await stockService.SyncData();
                });
               
                return true;
            });
        }

        protected override void OnResume()
        {
        }
    }
}