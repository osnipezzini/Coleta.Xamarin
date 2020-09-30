using AppColeta.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppColeta
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void Button_OnClicked(object sender, EventArgs e) => await OpenScan();

        private async Task OpenScan()
        {
            var scanner = DependencyService.Get<IQrCodeScanningService>();
            var result = await scanner.ScanAsync();
            if (!string.IsNullOrEmpty(result))
            {
                // Sua logica.
                var QrCode = result;
                await Navigation.PopAsync();
                lblCodigo.Text = result;
            }
        }
    }
}
