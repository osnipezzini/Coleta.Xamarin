using SOColeta.Models;
using SOColeta.ViewModels;

using System;

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

            viewModel.OnFinishedReadCode += OnFinishedReadCode;
        }

        private void BuscarCodigo(object sender, FocusEventArgs e)
        {
            ///viewModel.GetCodigo();
        }
        private void OnFinishedReadCode(object sender, EventArgs e) => txtQuantity.Focus();
    }
}