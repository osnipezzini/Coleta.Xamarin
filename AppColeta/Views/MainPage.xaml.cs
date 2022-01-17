using SOColeta.ViewModels;

using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage
    {
        private readonly MainViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();

            BindingContext = viewModel = Module.GetService<MainViewModel>();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }
    }
}