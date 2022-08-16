using SOColeta.Views;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            Title = "Login";
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
    }
}
