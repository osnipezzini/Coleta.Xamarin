using AppColeta.Droid.Services;
using AppColeta.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;

[assembly: Dependency(typeof(QrCodeScanningService))]
namespace AppColeta.Droid.Services
{
    public class QrCodeScanningService : IQrCodeScanningService
    {
        public async Task<string> ScanAsync()
        {
            var optionsCustom = new MobileBarcodeScanningOptions()
            {
                //UseFrontCameraIfAvailable = true
            };
            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Aproxime a câmera do código de barra",
                BottomText = "Toque na tela para focar"
            };

            var scanResults = await scanner.Scan(optionsCustom);

            return (scanResults != null) ? scanResults.Text : string.Empty;
        }
    }
}
