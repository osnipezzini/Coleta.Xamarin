using SOColeta.Data;
using SOColeta.Models;
using SOColeta.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using SOTech.Mvvm;

namespace SOColeta.ViewModels
{
    public class CriarColetaViewModel : ViewModelBase
    {
        private string codigo;
        private string quantidade;
        private string nome;
        private double precoVenda;
        private double precoCompra;
        private readonly IDataStore<Coleta> dataStore;

        public CriarColetaViewModel(IDataStore<Coleta> dataStore)
        {
            Title = "Criar coleta";
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            ReadCodeCommand = new Command(OpenScan);
            GetCodigoCommand = new Command(async() => await GetCodigo());
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            this.dataStore = dataStore;
        }
        private async void OpenScan(object obj)
        {
            
            var scanner = DependencyService.Get<IQrCodeScanningService>();
            var result = await scanner.ScanAsync();
            if (!string.IsNullOrEmpty(result))
            {
                // Sua logica.
                Codigo = result;
                return;
            }
        }
        async Task GetCodigo()
        {
            var contexto = new AppDbContext();
            var produto = await contexto.Produtos.Where(x => x.Codigo == codigo).FirstOrDefaultAsync();
            if (produto != null)
            {
                PrecoCusto = produto.PrecoCusto;
                Nome = produto.Nome;
                PrecoVenda = produto.PrecoVenda;
            }
        }
        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(codigo)
                && !String.IsNullOrWhiteSpace(quantidade) && double.TryParse(quantidade, out double valor);
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
            Coleta newItem = new Coleta()
            {
                Id = Guid.NewGuid().ToString(),
                Codigo = codigo,
                Quantidade = double.Parse(quantidade),
                InventarioId = App.Inventario.Id,
                Inventario = App.Inventario
            };

            await dataStore.AddItemAsync(newItem);

            Codigo = string.Empty;
            Quantidade = string.Empty;
        }
    }
}
