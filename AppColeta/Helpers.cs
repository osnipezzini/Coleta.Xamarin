using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta
{
    public static class Helpers
    {
        public static async Task ErrorMessage(string message, string title = "Erro") => await Application.Current.MainPage.DisplayAlert(title, message, "Ok");
        public static async Task InfoMessage(string message, string title = "Sucesso") => await Application.Current.MainPage.DisplayAlert(title, message, "Ok");
        public static async Task WarnMessage(string message, string title = "Atenção") => await Application.Current.MainPage.DisplayAlert(title, message, "Ok");
    }
}
