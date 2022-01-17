using SOColeta.ViewModels;

using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigPage
    {
        private readonly ConfigViewModel viewModel;
        public ConfigPage()
        {
            InitializeComponent();
            BindingContext = viewModel = Module.GetService<ConfigViewModel>();
        }
    }
}