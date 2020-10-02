using AppColeta.ViewModels;
using Xamarin.Forms;

namespace AppColeta.Views
{
    public partial class ColetaDetailPage : ContentPage
    {
        public ColetaDetailPage()
        {
            InitializeComponent();
            BindingContext = new ColetaDetailViewModel();
        }
    }
}