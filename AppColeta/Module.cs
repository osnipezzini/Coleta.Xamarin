using Microsoft.Extensions.DependencyInjection;

using SOColeta.Data;
using SOColeta.Services;

using SOFramework;

namespace SOColeta
{
    public static class Module
    {
        public const string AppName = "SOColeta";
        public const string AppId = "9B8D8134-E3E5-4C35-9900-86C920732C8A";
        public const string AppCenter = "android=22d5ba01-c5fb-42f1-ab9c-df098519a182;" +
                  "uwp=17841cc0-4811-4e23-ae1c-6aa8c5afe9f9;" +
                  "ios=cc9cf015-9548-4a26-a5f6-862bdf1b0d45;";

        private static IServiceCollection _services = new ServiceCollection();

        public static void Init()
        {
            _services = new ServiceCollection();

            #region SOTech Internals
            _services.UseSOLicense();
            _services.RegisterViews(typeof(Module));
            #endregion

            #region Serviços
            _services.AddDbContext<AppDbContext>();
            _services.AddSingleton<IConfigService, ConfigService>();
            _services.AddScoped<IStockService, StockService>();
            _services.AddScoped<IQrCodeScanningService, BarcodeScanningService>();
            _services.AddLogging();
            #endregion
        }

        public static T GetService<T>()
        {
            return _services.BuildServiceProvider().GetService<T>();
        }
    }
}