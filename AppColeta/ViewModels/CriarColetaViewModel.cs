﻿using SOColeta.Common.Models;
using SOColeta.Services;

using SOTech.Mvvm;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    public delegate void FinishedReadCodeDelegate(object sender, EventArgs e);
    [QueryProperty(nameof(InventarioGuid), nameof(inventarioGuid))]
    public class CriarColetaViewModel : ViewModelBase
    {
        private string codigo;
        private string quantidade;
        private string nome;
        private double? precoVenda;
        private double? precoCompra;
        public string inventarioGuid;
        private Product product;

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
            product = await stockService.GetProduto(Codigo);

            if (product != null)
            {
                PrecoCusto = product.CostPrice;
                Nome = product.Name;
                PrecoVenda = product.SalePrice;
            }
        }
        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(codigo)
                && !string.IsNullOrWhiteSpace(quantidade) && double.TryParse(quantidade, out double valor);
        }

        public Product Product
        {
            get => product;
            set => SetProperty(ref product, value);
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
        public double? PrecoCusto
        {
            get => precoCompra;
            set => SetProperty(ref precoCompra, value);
        }
        public double? PrecoVenda
        {
            get => precoVenda;
            set => SetProperty(ref precoVenda, value);
        }

        public string Quantidade
        {
            get => quantidade;
            set => SetProperty(ref quantidade, value);
        }
        public int? InventarioId { get; set; }
        public string InventarioGuid { get => inventarioGuid; set => SetProperty(ref inventarioGuid, value); }
        public event FinishedReadCodeDelegate OnFinishedReadCode;

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command ReadCodeCommand { get; }
        public Command GetCodigoCommand { get; }

        public override Task OnAppearing(string payload)
        {
            return base.OnAppearing(payload);
        }

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
                InventarioGuid = Guid.Parse(InventarioGuid),
            }); ;

            Codigo = string.Empty;
            Quantidade = string.Empty;
        }
    }
}
