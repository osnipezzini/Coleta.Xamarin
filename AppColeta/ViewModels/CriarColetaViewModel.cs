using SOColeta.Models;
using SOColeta.Services;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    public delegate void FinishedReadCodeDelegate(object sender, EventArgs e);
    [QueryProperty(nameof(InventarioId), nameof(InventarioId))]
    public class CriarColetaViewModel : ViewModelBase
    {
        private string codigo;
        private string quantidade;
        private string nome;
        private double precoVenda;
        private double precoCompra;

        private readonly IStockService stockService;

        public CriarColetaViewModel(IStockService stockService)
        {
            Title = "Criar coleta";
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            ReadCodeCommand = new Command(OpenScan);
            GetCodigoCommand = new Command(async () => await GetCodigo());
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            this.stockService = stockService;
        }
        private async void OpenScan(object obj)
        {

            var scanner = DependencyService.Get<IQrCodeScanningService>();
            var result = await scanner.ScanAsync();
            if (!string.IsNullOrEmpty(result))
            {
                // Sua logica.
                Codigo = result;
                OnFinishedReadCode?.Invoke(this, null);
                return;
            }
        }
        public async Task GetCodigo()
        {
            var produto = await stockService.GetProduto(Codigo);

            if (produto != null)
            {
                PrecoCusto = produto.PrecoCusto;
                Nome = produto.Nome;
                PrecoVenda = produto.PrecoVenda;
            }
        }
        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(codigo)
                && !string.IsNullOrWhiteSpace(quantidade) && double.TryParse(quantidade, out double valor);
        }

        public string Codigo
        {
            get => codigo;
            set => SetProperty(ref codigo, value);
        }
        public string Nome
        {
            get => nome;
            set => SetProperty(ref nome, value);
        }
        public double PrecoCusto
        {
            get => precoCompra;
            set => SetProperty(ref precoCompra, value);
        }
        public double PrecoVenda
        {
            get => precoVenda;
            set => SetProperty(ref precoVenda, value);
        }

        public string Quantidade
        {
            get => quantidade;
            set => SetProperty(ref quantidade, value);
        }
        public string InventarioId { get; set; }
        public event FinishedReadCodeDelegate OnFinishedReadCode;

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command ReadCodeCommand { get; }
        public Command GetCodigoCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            await stockService.AddColeta(new Coleta
            {
                Codigo = codigo,
                Quantidade = double.Parse(quantidade),
                InventarioId = InventarioId
            });

            Codigo = string.Empty;
            Quantidade = string.Empty;
        }
    }
}
