using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Extensions.Logging;

using SOColeta.Data;
using SOColeta.Services;
using SOColeta.ViewModels;

using SOCore;

using System;
using System.Collections.ObjectModel;

namespace SOColeta
{
    public static class Module
    {
        public const string AppName = "SOColeta";
        public const string AppId = "9B8D8134-E3E5-4C35-9900-86C920732C8A";
        public const string AppCenterKey = "android=22d5ba01-c5fb-42f1-ab9c-df098519a182;" +
                  "uwp=17841cc0-4811-4e23-ae1c-6aa8c5afe9f9;" +
                  "ios=cc9cf015-9548-4a26-a5f6-862bdf1b0d45;";
        private static IServiceProvider serviceProvider;
        private static IServiceCollection _services = new ServiceCollection();
        private static IServiceProvider ServiceProvider => serviceProvider;
        public static void Init()
        {
            AppCenter.Start(AppCenterKey, typeof(Analytics), typeof(Crashes));
            _services = new ServiceCollection();

            #region SOTech Internals
            _services.AddSOCore();
            #endregion

            #region ViewModels
            _services.AddScoped<ColetaDetailViewModel>();
            _services.AddScoped<ConfigViewModel>();
            _services.AddScoped<CriarColetaViewModel>();
            _services.AddScoped<CriarInventarioViewModel>();
            _services.AddScoped<ImportFileViewModel>();
            _services.AddScoped<LicenseViewModel>();
            _services.AddScoped<LoginViewModel>();
            _services.AddScoped<MainViewModel>();
            _services.AddScoped<MeusInventariosViewModel>();
            #endregion

            #region ViewModels
            _services.AddScoped<ColetaDetailViewModel>();
            _services.AddScoped<ConfigViewModel>();
            _services.AddScoped<CriarColetaViewModel>();
            _services.AddScoped<CriarInventarioViewModel>();
            _services.AddScoped<ImportFileViewModel>();
            _services.AddScoped<LicenseViewModel>();
            _services.AddScoped<LoginViewModel>();
            _services.AddScoped<MainViewModel>();
            _services.AddScoped<MeusInventariosViewModel>();
            #endregion

            #region Serviços
            _services.AddDbContext<AppDbContext>();
            _services.AddScoped<IStockService, StockService>();
_services.AddLogging();

#endregion

            _services.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory(Log.Logger));

            serviceProvider = _services
                .BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return ServiceProvider
                .GetRequiredService<T>();
        }
    }
}