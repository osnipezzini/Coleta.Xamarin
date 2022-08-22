﻿using SOColeta.Exceptions;
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
        private readonly IQrCodeScanningService scanningService;

        public CriarColetaViewModel(IStockService stockService, IQrCodeScanningService scanningService)
        {
            Title = "Criar coleta";
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            ReadCodeCommand = new Command(OpenScan);
            GetCodigoCommand = new Command(async () => await GetCodigo());
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            this.stockService = stockService;
            this.scanningService = scanningService;
        }
        private async void OpenScan(object obj)
        {
            var result = await scanningService.ScanAsync();
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
            var coleta = new Coleta
            {
                Codigo = codigo,
                Quantidade = double.Parse(quantidade),
                InventarioId = InventarioId
            };
            try
            {
                await stockService.AddColeta(coleta);

                Codigo = string.Empty;
                Quantidade = string.Empty;
            }
            catch (ColetaConflictException cce)
            {
                var resposta = await Shell.Current.DisplayActionSheet("Coleta já existe, o que deseja fazer?", "", "", "Somar", "Substituir");
                if (resposta != "Cancelar" && resposta != "Sair")
                    await stockService.AddColeta(coleta, resposta == "Substituir");

                Codigo = string.Empty;
                Quantidade = string.Empty;
            }

            Codigo = string.Empty;
            Quantidade = string.Empty;
        }
    }
}
