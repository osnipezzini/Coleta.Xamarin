using SOColeta.Models;
using SOColeta.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeusInventariosPage
    {
        private readonly MeusInventariosViewModel viewModel;
        public MeusInventariosPage()
        {
            InitializeComponent();

            BindingContext = viewModel = Module.GetService<MeusInventariosViewModel>();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
            viewModel.SelectedItem = null;            
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            viewModel.SelectedItem = (Inventario)e.SelectedItem;
        }
    }
}