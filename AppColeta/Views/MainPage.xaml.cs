using AppColeta.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        MainViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MainViewModel();
        }
    }
}