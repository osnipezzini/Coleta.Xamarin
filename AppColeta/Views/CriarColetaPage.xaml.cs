using SOColeta.Common.Models;
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

        private async void BuscarCodigo(object sender, FocusEventArgs e)
        {
            await viewModel.GetCodigo();
        }
        private void OnFinishedReadCode(object sender, EventArgs e) => txtQuantity.Focus();

        private void OnFinishedQuantity(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Codigo) || string.IsNullOrEmpty(viewModel.Quantidade))
                return;

            viewModel.SaveCommand.Execute(null);
            txtCode.Focus();
        }

        private void OnPlusButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Quantidade))
                viewModel.Quantidade = 1.00.ToString("N2");
            else if (double.TryParse(viewModel.Quantidade, out double quantidade))
            {
                var quantity = quantidade + 1;
                viewModel.Quantidade = quantity.ToString("N2");
            }
        }

        private void OnMinusButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(viewModel.Quantidade)
                && double.TryParse(viewModel.Quantidade, out double quantidade) && quantidade > 0)
            {
                var quantity = quantidade - 1;
                viewModel.Quantidade = quantity > 0 ? quantity.ToString("N2") : string.Empty;
            }
        }
    }
}