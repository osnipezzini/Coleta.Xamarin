using SOColeta.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColetaConfigPage : ContentPage
    {
        private readonly ColetaConfigViewModel viewModel;

        public ColetaConfigPage()
        {
            InitializeComponent();
            BindingContext = viewModel = Module.GetService<ColetaConfigViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.OnAppearing();
        }
    }
}