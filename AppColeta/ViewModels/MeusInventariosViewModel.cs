using AppColeta.Data;
using AppColeta.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppColeta.ViewModels
{
    class MeusInventariosViewModel : BaseViewModel
    {
        private string nomeArquivo;
        private DateTime dataCriacao;
        public ObservableCollection<Inventario> Inventarios { get; }
        public Command ExportFileCommand { get; }

        internal async Task OnAppearing()
        {
            Inventarios.Clear();
            IsBusy = true;
            try
            {
                var contexto = new AppDbContext();
                var inventarios = await contexto.Inventarios.ToListAsync();
                foreach (Inventario inventario in inventarios)
                {
                    Inventarios.Add(inventario);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command LoadInventariosCommand { get; set; }
        public MeusInventariosViewModel()
        {
            Title = "Meus inventários";
            ExportFileCommand = new Command(async () => await ExecuteExportFileCommand());
            LoadInventariosCommand = new Command(ExecuteLoadInventariosCommand);
            Inventarios = new ObservableCollection<Inventario>();
        }

        public async void ExecuteLoadInventariosCommand(object obj) => await OnAppearing();
        private async Task ExecuteExportFileCommand()
        {

        }


        private bool CanExportFile(object arg)
        {
            throw new NotImplementedException();
        }

        public string NomeArquivo { get => nomeArquivo; set => SetProperty(ref nomeArquivo, value); }
        public DateTime DataCriacao { get => dataCriacao; set => SetProperty(ref dataCriacao, value); }
    }
}
