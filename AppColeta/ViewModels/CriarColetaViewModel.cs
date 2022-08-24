﻿using SOColeta.Exceptions;
using SOColeta.Models;
using SOColeta.Services;

using SOFramework.Fonts;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    public delegate void FinishedReadCodeDelegate(object sender, EventArgs e);
    [QueryProperty(nameof(InventarioId), nameof(InventarioId))]
    [QueryProperty(nameof(ColetaId), nameof(ColetaId))]
    public class CriarColetaViewModel : ViewModelBase
    {
        private string codigo;
        private string quantidade = "1";
        private string nome;
        private double precoVenda;
        private double precoCompra;
        private bool isEditing = false;
        private string saveButtonName = "Digitar código";
        private string saveButtonIcon = FASolid.Keyboard;

        private readonly IStockService stockService;
        private readonly IQrCodeScanningService scanningService;

        public ObservableCollection<Coleta> LastColetas { get; }

        public CriarColetaViewModel(IStockService stockService, IQrCodeScanningService scanningService)
        {
            Title = "Coleta";
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);            
            ReadCodeCommand = new Command(OpenScan);
            GetCodigoCommand = new Command(async () => await GetCodigo());
            GetLastColetasCommand = new Command(async () => await GetLastColetas());
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            this.stockService = stockService;
            this.scanningService = scanningService;

            LastColetas = new ObservableCollection<Coleta>();
        }

        private async Task GetLastColetas()
        {
            LastColetas.Clear();

            await foreach (var inventario in stockService.GetColetasAsync(InventarioId, 3))
            {
                LastColetas.Add(inventario);
            }
        }

        public override async Task OnAppearing()
        {
            if (!string.IsNullOrEmpty(ColetaId))
            {
                IsEditing = true;
#if ANDROID
                using var loading = UserDialogs.Instance.Loading("Carregando coleta...");
#endif
                var coleta = await stockService.GetColetaAsync(ColetaId);
                Codigo = coleta.Codigo;
                Quantidade = coleta.Quantidade.ToString();
                InventarioId = coleta.InventarioId;
                await GetCodigo();
            }

            await GetLastColetas();
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
            set
            {
                SetProperty(ref codigo, value);
                if (!string.IsNullOrEmpty(value))
                {
                    SaveButtonName = IsEditing ? "Salvar" : "Adicionar";
                    SaveButtonIcon = FASolid.CheckCircle;
                }
                else
                {
                    SaveButtonName = "Digitar código";
                    SaveButtonIcon = FASolid.Keyboard;
                }
            }
        }
        public string Nome
        {
            get => nome;
            set => SetProperty(ref nome, value);
        }
        public string SaveButtonIcon
        {
            get => saveButtonIcon;
            set => SetProperty(ref saveButtonIcon, value);
        }
        public string SaveButtonName
        {
            get => saveButtonName;
            set => SetProperty(ref saveButtonName, value);
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
        public bool IsEditing
        {
            get => isEditing;
            set => SetProperty(ref isEditing, value);
        }

        public string Quantidade
        {
            get => quantidade;
            set => SetProperty(ref quantidade, value);
        }
        public string InventarioId { get; set; }
        public string ColetaId { get; set; }
        public event FinishedReadCodeDelegate OnFinishedReadCode;

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command ReadCodeCommand { get; }
        public Command GetCodigoCommand { get; }
        public Command GetLastColetasCommand { get; }

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
                Quantidade = double.TryParse(this.quantidade, out var quantidade) ? quantidade : 1,
                InventarioId = InventarioId
            };
            try
            {
                await stockService.RemoveColeta(coleta);
                await stockService.AddColeta(coleta, isEditing);
                await GetLastColetas();
            }
            catch (ColetaConflictException)
            {
                var resposta = await Shell.Current.DisplayActionSheet("Coleta já existe, o que deseja fazer?", "", "", "Somar", "Substituir");
                if (resposta != "Cancelar" && resposta != "Sair")
                    await stockService.AddColeta(coleta, resposta == "Substituir");
            }

            Codigo = string.Empty;
            Quantidade = "1";
        }
    }
}
