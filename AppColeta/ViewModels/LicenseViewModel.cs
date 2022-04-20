using Microsoft.Extensions.Logging;

using SOColeta.Views;

using SOCore.Exceptions;
using SOCore.Services;

using SOTech.Mvvm;

using System;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    public class LicenseViewModel : ViewModelBase
    {
        private string doc;
        private string password;
        private readonly ILicenseService licenseService;
        private readonly ILogger<LicenseViewModel> logger;

        public Command LicenseGenerateCommand { get; }

        public LicenseViewModel(ILicenseService licenseService, 
            ILogger<LicenseViewModel> logger, ISOCoreService coreService)
        {
            Title = "Licenciamento do sistema";
            LicenseGenerateCommand = new Command(OnLicenseGenerateClicked, CanGenerate);
            PropertyChanged += (_, __) => LicenseGenerateCommand.ChangeCanExecute();
            this.licenseService = licenseService;
            this.logger = logger;

            Serial = coreService.Serial;
        }

        private bool CanGenerate(object arg)
        {
            return !string.IsNullOrEmpty(password) && doc.Length > 11;
        }
        public bool New { get; set; } = true;
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string Document { get => doc; set => SetProperty(ref doc, value); }
        public string Serial { get; }

        private async void OnLicenseGenerateClicked(object obj)
        {
            IsBusy = true;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    await licenseService.RegisterDeviceAsync(Document, Password);
                    await GoToAsync($"///{nameof(MainPage)}");
                }
                catch (LicenseNotFoundException lre)
                {
                    logger.LogError(lre, "Erro ao registrar o dispositivo");
                    await DisplayErrorAsync(lre.Message);
                }
                catch (Exception exc)
                {
                    logger.LogError(exc, "Erro ao registrar o dispositivo");
                    await DisplayErrorAsync(".: ERRO FATAL :. \n" + exc.Message);
                }
            }
            else
            {
                await DisplayAlertAsync("Uma conexão ativa com a internet é necessária para licenciar o sistema.", "Sem Internet", "OK");
            }
            IsBusy = false;
        }
    }
}
