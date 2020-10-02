using AppColeta.Data;
using AppColeta.Models;
using AppColeta.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppColeta.ViewModels
{
    class MeusInventariosViewModel : BaseViewModel
    {
        private string nomeArquivo;
        private DateTime dataCriacao;
        private Inventario _selectedItem;
        public ObservableCollection<Inventario> Inventarios { get; }
        public Command ExportFileCommand { get; }
        public Command<Inventario> SelectedItemCommand { get; }
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
            ExportFileCommand = new Command(ExecuteExportFileCommand, CanExportFile);
            LoadInventariosCommand = new Command(ExecuteLoadInventariosCommand);
            Inventarios = new ObservableCollection<Inventario>();
            SelectedItemCommand = new Command<Inventario>(OnItemSelected);
            this.PropertyChanged +=
                (_, __) => ExportFileCommand.ChangeCanExecute();
        }

        private void OnItemSelected(Inventario obj)
        {
            if (obj == null)
                return;

            SelectedItem = obj;
        }

        public async void ExecuteLoadInventariosCommand(object obj) => await OnAppearing();
        private async void ExecuteExportFileCommand(object objeto)
        {
            var obj = SelectedItem;
            var arquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), obj.NomeArquivo);
            var arquivoString = "Codigo;Quantidade;Data;\n";
            var context = new AppDbContext();
            obj = await context.Inventarios.Include(x => x.ProdutosColetados).Where(x => x.Id == obj.Id).FirstOrDefaultAsync();
            if (obj != null)
            {
                foreach (Coleta coleta in obj.ProdutosColetados)
                {
                    arquivoString += $"{coleta.Codigo};{coleta.Quantidade};{obj.DataCriacao.ToString("dd/MM/yyyy")};\n";
                }
                File.WriteAllText(arquivo, arquivoString);
                await MailSender.SendEmail("Arquivo de inventário", "Você solicitou um arquivo de formulário, o mesmo se encontra em anexo", new List<string>(), arquivo);
            }
        }


        private bool CanExportFile(object arg)
        {
            return _selectedItem != null;
        }
        public Inventario SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                //OnItemSelected(value);
            }
        }
        
        public string NomeArquivo { get => nomeArquivo; set => SetProperty(ref nomeArquivo, value); }
        public DateTime DataCriacao { get => dataCriacao; set => SetProperty(ref dataCriacao, value); }
    }
}
