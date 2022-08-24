using Xamarin.Forms;

using ZXing.Mobile;

namespace SOColeta.Models
{
    public class Config
    {
        public int DelayBetweenFocus { get; set; } = 2;
        public SwipeMode SwipeMode { get; set; } = SwipeMode.Reveal;
        public MobileBarcodeScanningOptions BarcodeOptions { get; set; }
        public Config()
        {
            BarcodeOptions = new MobileBarcodeScanningOptions();
        }
    }
}
