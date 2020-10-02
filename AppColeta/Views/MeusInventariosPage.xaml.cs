using AppColeta.Models;
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
            viewModel.SelectedItem = null;
            base.OnAppearing();
            
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            viewModel.SelectedItem = (Inventario)e.SelectedItem;
        }
    }
}