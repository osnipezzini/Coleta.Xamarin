using SOColeta.Models;
using SOColeta.Services;

using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    public partial class ColetaConfigViewModel : ViewModelBase
    {
        private bool disableAutofocus = false;
        private bool autoRotate = false;
        private bool useFrontCameraIfAvailable = false;
        private bool? pureBarcode = null;
        private bool? assumeGs1 = null;
        private bool? useNativeScanning = null;
        private bool? tryHarder = null;
        private bool? tryInverted = null;
        private bool? code39ExtendedMode = null;
        private string characterSet = "";
        private int initialDelayBeforeAnalyzingFrames = 300;
        private int delayBetweenContinuousScans = 1000;
        private int delayBetweenAnalyzingFrames = 150;

        private int delayBetweenFocus = 2;

        private readonly IConfigService configService;

        public ColetaConfigViewModel(IConfigService configService)
        {
            this.configService = configService;

            SaveCommand = new Command(() =>
            {
                configService.SetConfig(GetConfig());
            });
        }

        public bool DisableAutoFocus { get => disableAutofocus; set => SetProperty(ref disableAutofocus, value); }
        public bool? PureBarcode { get => pureBarcode; set => SetProperty(ref pureBarcode, value); }
        public bool? TryHarder { get => tryHarder; set => SetProperty(ref tryHarder, value); }
        public bool AutoRotate { get => autoRotate; set => SetProperty(ref autoRotate, value); }
        public bool? AssumeGS1 { get => assumeGs1; set => SetProperty(ref assumeGs1, value); }
        public bool UseFrontCameraIfAvailable { get => useFrontCameraIfAvailable; set => SetProperty(ref useFrontCameraIfAvailable, value); }
        public bool? UseNativeScanning { get => useNativeScanning; set => SetProperty(ref useNativeScanning, value); }
        public bool? TryInverted { get => tryInverted; set => SetProperty(ref tryInverted, value); }
        public bool? UseCode39ExtendedMode { get => code39ExtendedMode; set => SetProperty(ref code39ExtendedMode, value); }
        public string CharacterSet { get => characterSet; set => SetProperty(ref characterSet, value); }
        public int InitialDelayBeforeAnalyzingFrames { get => initialDelayBeforeAnalyzingFrames; set => SetProperty(ref initialDelayBeforeAnalyzingFrames, value); }
        public int DelayBetweenAnalyzingFrames { get => delayBetweenAnalyzingFrames; set => SetProperty(ref delayBetweenAnalyzingFrames, value); }
        public int DelayBetweenContinuousScans { get => delayBetweenContinuousScans; set => SetProperty(ref delayBetweenContinuousScans, value); }
        public int DelayBetweenFocus { get => delayBetweenFocus; set => SetProperty(ref delayBetweenFocus, value); }

        public Command SaveCommand { get; }

        private void SetConfig(Config config)
        {
            DisableAutoFocus = config?.BarcodeOptions?.DisableAutofocus ?? false;
            PureBarcode = config?.BarcodeOptions?.PureBarcode;
            TryHarder = config?.BarcodeOptions?.TryHarder;
            TryInverted = config?.BarcodeOptions?.TryInverted;
            AutoRotate = config?.BarcodeOptions?.AutoRotate ?? false;
            AssumeGS1 = config?.BarcodeOptions?.AssumeGS1;
            UseFrontCameraIfAvailable = config?.BarcodeOptions?.UseFrontCameraIfAvailable ?? false;
            UseNativeScanning = config?.BarcodeOptions?.UseNativeScanning;
            UseCode39ExtendedMode = config?.BarcodeOptions?.UseCode39ExtendedMode;
            CharacterSet = config?.BarcodeOptions?.CharacterSet;
            InitialDelayBeforeAnalyzingFrames = config?.BarcodeOptions?.InitialDelayBeforeAnalyzingFrames ?? 300;
            DelayBetweenAnalyzingFrames = config?.BarcodeOptions?.DelayBetweenAnalyzingFrames ?? 150;
            DelayBetweenContinuousScans = config?.BarcodeOptions?.DelayBetweenContinuousScans ?? 1000;
            DelayBetweenFocus = config?.DelayBetweenFocus ?? 2;
        }

        private Config GetConfig()
        {
            var config = new Config();

            config.BarcodeOptions.AssumeGS1 = AssumeGS1;
            config.BarcodeOptions.AutoRotate = AutoRotate;
            config.BarcodeOptions.CharacterSet = CharacterSet;
            config.BarcodeOptions.DelayBetweenAnalyzingFrames = DelayBetweenAnalyzingFrames;
            config.BarcodeOptions.DelayBetweenContinuousScans = DelayBetweenContinuousScans;
            config.BarcodeOptions.DisableAutofocus = DisableAutoFocus; ;
            config.BarcodeOptions.PureBarcode = PureBarcode;
            config.BarcodeOptions.InitialDelayBeforeAnalyzingFrames = InitialDelayBeforeAnalyzingFrames;
            config.BarcodeOptions.TryHarder = TryHarder;
            config.BarcodeOptions.TryInverted = TryInverted;
            config.BarcodeOptions.UseFrontCameraIfAvailable = UseFrontCameraIfAvailable;
            config.BarcodeOptions.UseNativeScanning = UseNativeScanning ?? false;
            config.BarcodeOptions.UseCode39ExtendedMode = UseCode39ExtendedMode;
            config.DelayBetweenFocus = DelayBetweenFocus;

            return config;
        }

        public override async Task OnAppearing()
        {
            var config = await configService.GetConfigAsync();

            SetConfig(config);
        }
    }
}
