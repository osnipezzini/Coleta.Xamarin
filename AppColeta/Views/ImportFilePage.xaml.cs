using SOColeta.ViewModels;

using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImportFilePage
    {
        private readonly ImportFileViewModel viewModel;
        public ImportFilePage()
        {
            InitializeComponent();
            BindingContext = viewModel = Module.GetService<ImportFileViewModel>();
        }
    }
}