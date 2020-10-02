using AppColeta.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarInventarioPage : ContentPage
    {
        CriarInventarioViewModel _viewModel;
        public CriarInventarioPage()
        {
            BindingContext = _viewModel = new CriarInventarioViewModel();
            InitializeComponent();            
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.ExecuteLoadColetasCommand();
        }
    }
}