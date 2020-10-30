using SOTechLib.Licensing;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppColeta.ViewModels
{
    public class LicenseViewModel : BaseViewModel
    {
        private string doc;
        private string password;

        public Command LicenseGenerateCommand { get; }

        public LicenseViewModel()
        {
            Title = "Licenciamento do sistema";
            LicenseGenerateCommand = new Command(OnLicenseGenerateClicked, CanGenerate);
            PropertyChanged += (_, __) => LicenseGenerateCommand.ChangeCanExecute();
        }

        private bool CanGenerate(object arg)
        {
            return !string.IsNullOrEmpty(password) && doc.Length > 11;
        }
        public bool New { get; set; } = true;
        public string Serial { get => LicenseHandler.GenerateUID("AppColetaMobile"); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string Document { get => doc; set => SetProperty(ref doc, value); }

        private async void OnLicenseGenerateClicked(object obj)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var isOk = await Helpers.GetLicense(doc, Serial, password);
                    if (isOk)
                        Application.Current.MainPage = new AppShell();
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
        }
    }
}
