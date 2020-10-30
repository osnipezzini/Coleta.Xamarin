using AppColeta.Data;
using AppColeta.Models;
using AppColeta.Services;
using AppColeta.Views;
using SOTechLib.Licensing.Models;
using System.IO;
using Xamarin.Forms;

namespace AppColeta
{
    public partial class App : Application
    {
        public static ERPLicense License { get; internal set; }
        public static Inventario Inventario { get; set; }
        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            string save_folder = DependencyService.Get<IAppPath>().Path;
            string license_file = Path.Combine(save_folder, "license.lic");
            if (File.Exists(license_file))
                MainPage = new AppShell();
            else
                MainPage = new LicensePage();
        }

        protected override void OnStart()
        {
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}