using SOColeta.Data;
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
        private readonly IDataStore<Coleta> dataStore;

        public ObservableCollection<Coleta> Coletas { get; set; }
        public Command LoadColetasCommand { get; }
        public Command IniciarColetaCommand { get; }
        public Command SaveCommand { get; }
        public CriarInventarioViewModel(IDataStore<Coleta> dataStore)
        {
            Title = "Criar inventario";
            LoadColetasCommand = new Command(async () => await ExecuteLoadColetasCommand());
            IniciarColetaCommand = new Command(async () => await ExecuteIniciarColetaCommand());
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            Coletas = new ObservableCollection<Coleta>();
            App.Inventario = new Inventario()
            {
                DataCriacao = DataCriacao = DateTime.Today,
                Id = Guid.NewGuid().ToString()
            };
            this.dataStore = dataStore;
        }

        private async Task ExecuteSaveCommand()
        {
            if (dataStore.Count > 0)
            {
                IsBusy = true;
                if (App.Inventario.ProdutosColetados == null)
                    App.Inventario.ProdutosColetados = new List<Coleta>();
                App.Inventario.ProdutosColetados.AddRange(await dataStore.GetItemsAsync());
                App.Inventario.DataCriacao = DataCriacao;
                App.Inventario.NomeArquivo = $"Arquivo-{DateTime.Now.ToString("ddMMyyyyHHmm")}.txt";
                var contexto = new AppDbContext();
                contexto.Inventarios.Add(App.Inventario);
                try
                {
                    if (await contexto.SaveChangesAsync() > 0)
                        await DisplayAlertAsync("Seu inventário foi salvo com sucesso", "Salvo");
                    else
                        await DisplayAlertAsync("Ocorreu um erro ao salvar seu inventário.", "Erro");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await DisplayAlertAsync(ex.Message, "ERRO FATAL");
                }
                finally
                {
                    App.Inventario = new Inventario()
                    {
                        DataCriacao = DataCriacao = DateTime.Today,
                        Id = Guid.NewGuid().ToString()
                    };
                    IsBusy = false;
                }

            }
            else
                await DisplayAlertAsync("Atenção: Proibido salvar inventários vazios!", "Acesso negado");
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
                var items = await dataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Coletas.Add(item);
                }
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
