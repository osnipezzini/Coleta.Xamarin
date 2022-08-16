namespace SOColeta.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private string license_footer;
        public string LicenseFooter { get => license_footer; private set => SetProperty(ref license_footer, value); }
        public MainViewModel()
        {
            string titulo = "SOColeta";
            Title = titulo;
        }
    }
}
