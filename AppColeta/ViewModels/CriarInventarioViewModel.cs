using AppColeta.Data;
using AppColeta.Models;
using AppColeta.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppColeta.ViewModels
{
    class CriarInventarioViewModel : BaseViewModel
    {
        private DateTime _dataCriacao;
        public ObservableCollection<Coleta> Coletas { get; set; }
        public Command LoadColetasCommand { get; }
        public Command IniciarColetaCommand { get; }
        public Command SaveCommand { get; }
        public CriarInventarioViewModel()
        {
            
            LoadColetasCommand = new Command(async () => await ExecuteLoadColetasCommand());
            IniciarColetaCommand = new Command(async () => await ExecuteIniciarColetaCommand());
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            Coletas = new ObservableCollection<Coleta>();
            App.Inventario = new Inventario()
            {
                DataCriacao = DataCriacao = DateTime.Today,
                Id = Guid.NewGuid().ToString()
            };
        }

        private async Task ExecuteSaveCommand()
        {
            if (ColetasStore.Count > 0)
            {
                IsBusy = true;
                if (App.Inventario.ProdutosColetados == null)
                    App.Inventario.ProdutosColetados = new List<Coleta>();
                App.Inventario.ProdutosColetados.AddRange(await ColetasStore.GetItemsAsync());
                App.Inventario.DataCriacao = DataCriacao;
                App.Inventario.NomeArquivo = $"Arquivo-{DateTime.Now.ToString("ddMMyyyyHHmm")}.txt";
                var contexto = new AppDbContext();
                contexto.Inventarios.Add(App.Inventario);
                try
                {
                    if (await contexto.SaveChangesAsync() > 0)
                        await Shell.Current.DisplayAlert("Salvo", "Seu inventário foi salvo com sucesso", "Ok");
                    else
                        await Shell.Current.DisplayAlert("Erro", "Ocorreu um erro ao salvar seu inventário.", "Ok");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await Shell.Current.DisplayAlert("ERRO FATAL", ex.Message, "Ok");
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
            await Shell.Current.GoToAsync($"{nameof(CriarColetaPage)}");
        }

        public DateTime DataCriacao { get => _dataCriacao; set => SetProperty(ref _dataCriacao, value); }
        public async Task ExecuteLoadColetasCommand()
        {
            IsBusy = true;
            try
            {
                Coletas.Clear();
                var items = await ColetasStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Coletas.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
