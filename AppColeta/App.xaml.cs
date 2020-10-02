using AppColeta.Models;
using AppColeta.Services;
using Xamarin.Forms;

namespace AppColeta
{
    public partial class App : Application
    {
        public static Inventario Inventario { get; set; }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
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
