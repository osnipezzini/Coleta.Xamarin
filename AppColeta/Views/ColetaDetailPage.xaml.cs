using SOColeta.ViewModels;

using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColetaDetailPage
    {
        private readonly ColetaDetailViewModel viewModel;
        public ColetaDetailPage()
        {
            InitializeComponent();

            BindingContext = viewModel = Module.GetService<ColetaDetailViewModel>();
        }
    }
}