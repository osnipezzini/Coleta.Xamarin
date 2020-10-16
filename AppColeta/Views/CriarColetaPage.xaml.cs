using AppColeta.Models;
using AppColeta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarColetaPage : ContentPage
    {
        CriarColetaViewModel viewModel => BindingContext as CriarColetaViewModel;
        public Coleta Coleta { get; set; }

        public CriarColetaPage()
        {
            InitializeComponent();
        }

        private void BuscarCodigo(object sender, FocusEventArgs e)
        {
            viewModel.GetCodigoCommand.Execute(sender);
        }
    }
}