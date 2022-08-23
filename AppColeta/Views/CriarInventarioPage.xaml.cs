using SOColeta.ViewModels;

using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarInventarioPage
    {
        private readonly CriarInventarioViewModel viewModel;
        public CriarInventarioPage()
        {
            InitializeComponent();
            BindingContext = viewModel = Module.GetService<CriarInventarioViewModel>();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }

        private void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.EditColetaCommand.Execute(e.Item);
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Any())
                viewModel.EditColetaCommand.Execute(e.CurrentSelection.First());
        }
    }
}