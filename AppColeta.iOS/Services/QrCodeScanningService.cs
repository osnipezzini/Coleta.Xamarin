using AppColeta.iOS.Services;
using AppColeta.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;

[assembly: Dependency(typeof(QrCodeScanningService))]
namespace AppColeta.iOS.Services
{
    public class QrCodeScanningService : IQrCodeScanningService
    {
        public async Task<string> ScanAsync()
        {

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Aproxime a câmera do código de barra",
                BottomText = "Toque na tela para focar"
            };
            var scanResults = await scanner.Scan();
            //Fix by Ale
            return (scanResults != null) ? scanResults.Text : string.Empty;

        }
    }
}