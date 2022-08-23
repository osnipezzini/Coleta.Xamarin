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

        private void OnExport(object sender, System.EventArgs e)
        {
            if (sender is MenuItem menuItem)
                viewModel.ExportFileCommand.Execute(menuItem.CommandParameter);
        }

        private void OnEdit(object sender, System.EventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.CommandParameter is Inventario inventario)
            {
                Shell.Current.GoToAsync($"//CriarInventario?InventarioId={inventario.Id}");
            }
        }
    }
}