using SOColeta.Models;
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

        public ObservableCollection<Coleta> Coletas { get; set; }
        public Command LoadColetasCommand { get; }
        public Command IniciarColetaCommand { get; }
        public Command SaveCommand { get; }
        public CriarInventarioViewModel(IStockService stockService)
        {
            Title = "Criar inventario";
            LoadColetasCommand = new Command(async () => await ExecuteLoadColetasCommand());
            IniciarColetaCommand = new Command(async () => await ExecuteIniciarColetaCommand());
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            Coletas = new ObservableCollection<Coleta>();
            this.stockService = stockService;
        }

        private async Task ExecuteSaveCommand()
        {
            if (!await stockService.InventarioHasColeta())
                await DisplayAlertAsync("Atenção: Proibido finalizar inventários vazios!", "Acesso negado");
            else
            {
                IsBusy = true;

                try
                {
                    await stockService.FinishInventario();
                    await DisplayAlertAsync("Seu inventário foi salvo com sucesso", "Salvo");
                }
                catch (Exception ex)
                {
                    Logger.Debug($"===== {ex.GetType().FullName} =====");
                    Logger.Debug(ex.StackTrace);
                    Logger.Debug($"===== {ex.GetType().FullName} =====");
                    Logger.Error(ex.Message);
                    await DisplayAlertAsync(ex.Message, "ERRO FATAL");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task ExecuteIniciarColetaCommand()
        {
            await GoToAsync($"{nameof(CriarColetaPage)}");
        }

        public DateTime DataCriacao { get => _dataCriacao; set => SetProperty(ref _dataCriacao, value); }
        public async Task ExecuteLoadColetasCommand()
        {
            IsBusy = true;
            try
            {
                Coletas.Clear();
                await foreach (var coleta in stockService.GetColetasAsync())
                    if (coleta is not null)
                        Coletas.Add(coleta);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Logger.Debug(ex.StackTrace);
                Logger.Error(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
