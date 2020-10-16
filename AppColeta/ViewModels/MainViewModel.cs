using SOTechLib.Utils;

namespace AppColeta.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            string titulo = "AppColeta";
            if (Helper.TKT)
                titulo += " - TKT";
            if (Helper.Debug)
                titulo += " - Debug";
            Title = titulo;
        }
    }
}
