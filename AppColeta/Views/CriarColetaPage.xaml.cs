using SOColeta.Models;
using SOColeta.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarColetaPage
    {
        private readonly CriarColetaViewModel viewModel;
        public Coleta Coleta { get; set; }

        public CriarColetaPage()
        {
            InitializeComponent();

            BindingContext = viewModel = Module.GetService<CriarColetaViewModel>();
        }

        private void BuscarCodigo(object sender, FocusEventArgs e)
        {
            viewModel.GetCodigoCommand.Execute(sender);
        }
    }
}