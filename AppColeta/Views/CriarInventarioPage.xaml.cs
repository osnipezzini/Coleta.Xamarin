using AppColeta.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarInventarioPage : ContentPage
    {
        CriarInventarioViewModel viewModel => BindingContext as CriarInventarioViewModel;
        public CriarInventarioPage()
        {
            InitializeComponent();            
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.ExecuteLoadColetasCommand();
        }
    }
}