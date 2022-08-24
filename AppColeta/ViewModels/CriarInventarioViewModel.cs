using Microsoft.Extensions.Logging;

using SOColeta.Models;
using SOColeta.Services;
using SOColeta.Views;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    [QueryProperty(nameof(InventarioId), nameof(InventarioId))]
    internal class CriarInventarioViewModel : ViewModelBase
    {
        private DateTime _dataCriacao;
        private readonly IStockService stockService;
        private readonly ILogger<CriarInventarioViewModel> logger;
        private Inventario inventario;
        private string inventarioName;

        public ObservableCollection<Coleta> Coletas { get; set; }
        public Command LoadColetasCommand { get; }
        public Command IniciarColetaCommand { get; }
        public Command SaveCommand { get; }
        public Command SaveInventarioNameCommand { get; }
        public Command EditColetaCommand { get; }

        public CriarInventarioViewModel(IStockService stockService, ILogger<CriarInventarioViewModel> logger)
        {
            Title = "Criar inventario";
            LoadColetasCommand = new Command(async () => await ExecuteLoadColetasCommand());
            IniciarColetaCommand = new Command(async () => await ExecuteIniciarColetaCommand());
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            SaveInventarioNameCommand = new Command<string>(SetInventarioName);
            EditColetaCommand = new Command(EditColeta);
            Coletas = new ObservableCollection<Coleta>();
            this.stockService = stockService;
            this.logger = logger;
        }

        private void EditColeta(object obj)
        {
            if (obj is Coleta coleta)
                GoToAsync($"{nameof(CriarColetaPage)}?ColetaId={coleta.Id}");
        }

        public override async Task OnAppearing()
        {
            IsBusy = true;

            if (!string.IsNullOrEmpty(InventarioId))
            {
                inventario = await stockService.GetInventario(InventarioId);
            }
            else
            {
                inventario = await stockService.GetOpenedInventario();
                inventario ??= await stockService.CreateInventario();
            }
            Coletas.Clear();
            DataCriacao = inventario.DataCriacao;
            InventarioName = Path.HasExtension(inventario.NomeArquivo) ? Path.GetFileNameWithoutExtension(inventario.NomeArquivo) : inventario.NomeArquivo;

            IsBusy = false;

            LoadColetasCommand.Execute(null);
        }
        private async Task ExecuteSaveCommand()
        {
            if (string.IsNullOrEmpty(InventarioId) && !await stockService.InventarioHasColeta())
                await DisplayAlertAsync("Atenção: Proibido finalizar inventários vazios!");
            else
            {
                IsBusy = true;
                try
                {
                    if (string.IsNullOrEmpty(InventarioId))
                        await stockService.FinishInventario();
                    await DisplayAlertAsync("Seu inventário foi salvo com sucesso");
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"===== {ex.GetType().FullName} =====");
                    logger.LogDebug(ex.StackTrace);
                    logger.LogDebug($"===== {ex.GetType().FullName} =====");
                    logger.LogError(ex, "Erro ao salvar o inventário");
                    await DisplayAlertAsync(ex.Message);
                }
                finally
                {
                    InventarioId = string.Empty;
                    await OnAppearing();
                }
                IsBusy = false;
            }
        }

        private async Task ExecuteIniciarColetaCommand()
        {
            await GoToAsync($"/{nameof(CriarColetaPage)}?InventarioId={inventario.Id}");
        }
        public string InventarioId { get; set; }
        public string InventarioName { get => inventarioName; set => SetProperty(ref inventarioName, value); }
        public DateTime DataCriacao { get => _dataCriacao; set => SetProperty(ref _dataCriacao, value); }
        public async Task ExecuteLoadColetasCommand()
        {
            IsBusy = true;
            try
            {
                Coletas.Clear();

                if (inventario is null || string.IsNullOrEmpty(inventario.Id))
                    return;

                var coletas = stockService.GetColetasAsync(inventario.Id);
                await foreach (var coleta in coletas)
                    if (coleta is not null)
                        Coletas.Add(coleta);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                logger.LogDebug(ex.StackTrace);
                logger.LogError(ex, "Erro ao carregar as coletas do inventário");
            }
            IsBusy = false;
        }

        private void SetInventarioName(string name)
        {
            stockService.SetInventarioName(inventario, name);
        }
    }
}
