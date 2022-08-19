using System.Threading.Tasks;

using Xamarin.Forms;

using ZXing.Mobile;

namespace SOColeta.Services
{
    internal class BarcodeScanningService : IQrCodeScanningService
    {
        private readonly IConfigService configService;

        public BarcodeScanningService(IConfigService configService)
        {
            this.configService = configService;
        }
        public async Task<string> ScanAsync()
        {
            var config = await configService.GetConfigAsync();
            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Aproxime a câmera do código de barra",
                BottomText = "Toque na tela para focar"
            };
            bool isScanning = true;
            Device.StartTimer(new System.TimeSpan(0, 0, config.DelayBetweenFocus), () =>
            {
                scanner.AutoFocus();
                return isScanning;
            });
            scanner.AutoFocus();
            var scanResults = await scanner.Scan(config.BarcodeOptions);
            isScanning = false;
            return (scanResults != null) ? scanResults.Text : string.Empty;
        }
    }
}
