using SOFramework;

using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    public class ViewModelBase : SOViewModel
    {
        const string TitleAlerts = "SOColeta - Aviso";
        const string TitleErrorAlerts = "SOColeta - Falha ao executar";
        const string ButtonAlerts = "Ok";
        public Task GoToAsync(string route) => Shell.Current.GoToAsync(route);
        public Task DisplayAlertAsync(string message) => Shell.Current.DisplayAlert(TitleAlerts, message, ButtonAlerts);
        public Task DisplayErrorAsync(string message) => Shell.Current.DisplayAlert(TitleErrorAlerts, message, ButtonAlerts);
    }
}
