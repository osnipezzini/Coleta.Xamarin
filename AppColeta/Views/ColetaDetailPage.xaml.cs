using AppColeta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColetaDetailPage : ContentPage
    {
        ColetaDetailViewModel viewModel => BindingContext as ColetaDetailViewModel;
        public ColetaDetailPage()
        {
            InitializeComponent();
        }
    }
}