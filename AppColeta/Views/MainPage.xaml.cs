using AppColeta.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        MainViewModel viewModel => BindingContext as MainViewModel;
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.CheckValid();
        }
    }
}