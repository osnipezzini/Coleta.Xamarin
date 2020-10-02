using AppColeta.Models;
using AppColeta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarColetaPage : ContentPage
    {
        CriarColetaViewModel viewModel;
        public Coleta Coleta { get; set; }

        public CriarColetaPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CriarColetaViewModel();
        }
    }
}