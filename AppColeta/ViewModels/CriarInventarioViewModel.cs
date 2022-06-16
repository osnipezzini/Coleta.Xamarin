using Microsoft.Extensions.Logging;
using SOColeta.Common.Models;
using SOColeta.Services;
using SOColeta.Views;

using SOTech.Mvvm;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    internal class CriarInventarioViewModel : ViewModelBase
    {
        private DateTime _dataCriacao;
        private readonly IStockService stockService;
        private readonly ILogger<CriarInventarioViewModel> logger;
        private Inventario inventario;

        public ObservableCollection<Coleta> Coletas { get; set; }
        public Command LoadColetasCommand { get; }
        public Command IniciarColetaCommand { get; }
        public Command SaveCommand { get; }

        public CriarInventarioViewModel(IStockService stockService, ILogger<CriarInventarioViewModel> logger)
        {
            Title = "Criar inventario";
            LoadColetasCommand = new Command(async () => await ExecuteLoadColetasCommand());
            IniciarColetaCommand = new Command(async () => await ExecuteIniciarColetaCommand());
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            Coletas = new ObservableCollection<Coleta>();
            this.stockService = stockService;
            this.logger = logger;
        }
        public override async Task OnAppearing()
        {
            IsBusy = true;
            inventario = await stockService.GetOpenedInventario();
            if (inventario is null)
                inventario = await stockService.CreateInventario();

            DataCriacao = inventario.DataCriacao;
            if (await stockService.InventarioHasColeta())
                await ExecuteLoadColetasCommand();

            IsBusy = false;
        }
        private async Task ExecuteSaveCommand()
        {
            if (!await stockService.InventarioHasColeta())
                await DisplayAlertAsync("Acesso negado", "Atenção: Proibido finalizar inventários vazios!");
            else
            {
                IsBusy = true;
                try
                {
                    await stockService.FinishInventario();
                    await DisplayAlertAsync("Salvo", "Seu inventário foi salvo com sucesso");
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"===== {ex.GetType().FullName} =====");
                    logger.LogDebug(ex.StackTrace);
                    logger.LogDebug($"===== {ex.GetType().FullName} =====");
                    logger.LogError(ex, "Erro ao salvar o inventário");
                    await DisplayAlertAsync(ex.Message, "ERRO FATAL");
                }
                finally
                {
                    inventario = null;
                    await OnAppearing();
                }
            }
        }

        private async Task ExecuteIniciarColetaCommand()
        {
            await GoToAsync($"{nameof(CriarColetaPage)}?InventarioId={inventario.Id}");
        }

        public DateTime DataCriacao { get => _dataCriacao; set => SetProperty(ref _dataCriacao, value); }
        public async Task ExecuteLoadColetasCommand()
        {
            try
            {
                if (inventario.Guid == null)
                    return;
                Coletas.Clear();
                await foreach (var coleta in stockService.GetColetasAsync(inventario.Guid))
                    if (coleta is not null)
                        Coletas.Add(coleta);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                logger.LogDebug(ex.StackTrace);
                logger.LogError(ex, "Erro ao carregar as coletas do inventário");
            }
        }
    }
}
