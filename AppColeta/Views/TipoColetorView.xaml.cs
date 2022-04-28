
using SOColeta.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TipoColetorView : ContentView
    {
        internal static TipoASColeta? TipoASColeta { get; private set; }
        public TipoColetorView()
        {
            InitializeComponent();
        }

        private void SelectedOpticon(object sender, System.EventArgs e)
        {
            TipoASColeta = Models.TipoASColeta.Opticon;
            Shell.Current.GoToAsync("..");
        }
        private void SelectedCipherLab(object sender, System.EventArgs e)
        {
            TipoASColeta = Models.TipoASColeta.CipherLab;
            Shell.Current.GoToAsync("..");
        }
    }
}