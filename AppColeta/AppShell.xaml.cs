using AppColeta.Views;
using Xamarin.Forms;

namespace AppColeta
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(CriarInventarioPage), typeof(CriarInventarioPage));
            Routing.RegisterRoute(nameof(CriarColetaPage), typeof(CriarColetaPage));
            Routing.RegisterRoute(nameof(MeusInventariosPage), typeof(MeusInventariosPage));
            Routing.RegisterRoute(nameof(ConfigPage), typeof(ConfigPage));
            Routing.RegisterRoute(nameof(LicensePage), typeof(LicensePage));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }        
    }
}
