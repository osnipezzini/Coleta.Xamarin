using SOColeta.Views;

using SOTech.Core.Services;
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

        public Command LicenseGenerateCommand { get; }

        public LicenseViewModel(ILicenseService licenseService)
        {
            Title = "Licenciamento do sistema";
            LicenseGenerateCommand = new Command(OnLicenseGenerateClicked, CanGenerate);
            PropertyChanged += (_, __) => LicenseGenerateCommand.ChangeCanExecute();
            this.licenseService = licenseService;
        }

        private bool CanGenerate(object arg)
        {
            return !string.IsNullOrEmpty(password) && doc.Length > 11;
        }
        public bool New { get; set; } = true;
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string Document { get => doc; set => SetProperty(ref doc, value); }

        private async void OnLicenseGenerateClicked(object obj)
        {
            IsBusy = true;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    await licenseService.GetLicenseAsync(doc, password);
                    await GoToAsync($"///{nameof(MainPage)}");
                }
                catch (Exception exc)
                {
                    await Application.Current.MainPage.DisplayAlert(".: ERRO FATAL :. \n" + exc.Message, "ERRO", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Uma conexão ativa com a internet é necessária para licenciar o sistema.", "Sem Internet", "OK");
            }
            IsBusy = false;
        }
    }
}
