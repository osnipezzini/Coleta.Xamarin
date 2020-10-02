using AppColeta.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeusInventariosPage : ContentPage
    {
        MeusInventariosViewModel viewModel;
        public MeusInventariosPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MeusInventariosViewModel();
        }
        protected async override void OnAppearing()
        {
            await viewModel.OnAppearing();
            base.OnAppearing();
            
        }
    }
}