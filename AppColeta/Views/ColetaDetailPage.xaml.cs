using AppColeta.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColetaDetailPage : ContentPage
    {
        public ColetaDetailPage()
        {
            InitializeComponent();
            BindingContext = new ColetaDetailViewModel();
        }
    }
}