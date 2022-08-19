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
            if (e.SelectedItem is Inventario inventario)
                viewModel.SelectedItem = inventario;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Inventario inventario)
                viewModel.SelectedItem = inventario;
        }

        private void OnExportEmsys(object sender, System.EventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.CommandParameter is Inventario inventario)
                viewModel.ExportInventarioFile(inventario, TipoSistema.EMSys);
        }

        private void OnExportAutoSystem(object sender, System.EventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.CommandParameter is Inventario inventario)
                viewModel.ExportInventarioFile(inventario);
        }

        private void OnExport(object sender, System.EventArgs e)
        {
            if (sender is MenuItem menuItem)
                viewModel.ExportFileCommand.Execute(menuItem.CommandParameter);
        }
    }
}