using AppColeta.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigPage : ContentPage
    {
        ConfigViewModel viewModel;
        public ConfigPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ConfigViewModel();
        }
    }
}