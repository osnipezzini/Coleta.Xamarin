using SOColeta.Views;

using Xamarin.Forms;

namespace SOColeta
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CriarColetaPage), typeof(CriarColetaPage));
            Routing.RegisterRoute(nameof(ConfigPage), typeof(ConfigPage));
        }
    }
}
