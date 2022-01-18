using SOColeta.Models;
using SOColeta.Services;
using SOColeta.Views;

using SOTech.Mvvm;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    class CriarInventarioViewModel : ViewModelBase
    {
        private DateTime _dataCriacao;
        private readonly IDatabase database;

        public ObservableCollection<Coleta> Coletas { get; set; }
        public Command LoadColetasCommand { get; }
        public Command IniciarColetaCommand { get; }
        public Command SaveCommand { get; }
        public CriarInventarioViewModel(IDatabase database)
        {
            Title = "Criar inventario";
            LoadColetasCommand = new Command(async () => await ExecuteLoadColetasCommand());
            IniciarColetaCommand = new Command(async () => await ExecuteIniciarColetaCommand());
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            Coletas = new ObservableCollection<Coleta>();
            App.Inventario = new Inventario()
            {
                DataCriacao = DataCriacao = DateTime.Today
            };
            this.database = database;
        }

        private async Task ExecuteSaveCommand()
        {
            if (await database.CountColetasByInventarioAsync(App.Inventario.Id) > 0)
            {
                IsBusy = true;

                App.Inventario.DataCriacao = DataCriacao;
                App.Inventario.NomeArquivo = $"Arquivo-{DateTime.Now.ToString("ddMMyyyyHHmm")}.txt";

                try
                {
                    if (await database.AddInventarioAsync(App.Inventario))
                        await DisplayAlertAsync("Salvo", "Seu inventário foi salvo com sucesso", "Ok");
                    else
                        await DisplayAlertAsync("Erro", "Ocorreu um erro ao salvar seu inventário.", "Ok");
                }
                catch (Exception ex)
                {
                    Logger.Debug(ex.StackTrace);
                    Logger.Error(ex, "Erro ao salvar inventario");
                    await DisplayAlertAsync("ERRO FATAL", ex.Message, "Ok");
                }
                finally
                {
                    IsBusy = false;
                }

            }
            else
                await Shell.Current.DisplayAlert("Acesso negado", "Atenção: Proibido salvar inventários vazios!", "Ok");
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
                await foreach (var coleta in database.GetColetasAsync(App.Inventario.Id))
                {
                    Coletas.Add(coleta);
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(ex.StackTrace);
                Logger.Error(ex, "Erro ao carregar coletas.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
