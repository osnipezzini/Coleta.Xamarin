using Microsoft.Extensions.Logging;

using SOColeta.Models;
using SOColeta.Services;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    internal class MeusInventariosViewModel : ViewModelBase
    {
        private string nomeArquivo;
        private DateTime dataCriacao;
        private Inventario _selectedItem;
        private readonly IStockService stockService;
        private readonly ILogger<MeusInventariosViewModel> logger;

        public ObservableCollection<Inventario> Inventarios { get; }
        public Command ExportFileCommand { get; }
        public Command<Inventario> SelectedItemCommand { get; }
        public Command<Inventario> EditInventarioCommand { get; }

        public override async Task OnAppearing()
        {
            Inventarios.Clear();

            IsBusy = true;
            try
            {
                await foreach (var inventario in stockService.GetFinishedInventarios())
                    Inventarios.Add(inventario);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao buscar os inventários finalizados");
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command LoadInventariosCommand { get; set; }
        public MeusInventariosViewModel(IStockService stockService, ILogger<MeusInventariosViewModel> logger)
        {
            Title = "Inventários finalizados";
            ExportFileCommand = new Command<Inventario>(async (x) =>
            {
                var tipoInventario = await Shell
                .Current
                .DisplayActionSheet("Selecione o sistema para exportação", "Cancelar", null, Enum.GetNames(typeof(TipoSistema)));
                if (!Enum.TryParse<TipoSistema>(tipoInventario, out var tipoSistema))
                    return;
                ExecuteExportFileCommand(x, tipoSistema);
            }, CanExportFile);
            LoadInventariosCommand = new Command(ExecuteLoadInventariosCommand);
            Inventarios = new ObservableCollection<Inventario>();
            SelectedItemCommand = new Command<Inventario>(OnItemSelected);
            EditInventarioCommand = new Command<Inventario>(async(x) => await EditInventario(x));
            PropertyChanged +=
                (_, __) => ExportFileCommand.ChangeCanExecute();
            this.stockService = stockService;
            this.logger = logger;
        }

        private async Task EditInventario(Inventario inventario)
        {
            await stockService.ReopenInventario(inventario);
            await Shell.Current.GoToAsync($"/CriarInventario?InventarioId={inventario.Id}");
        }

        private void OnItemSelected(Inventario obj)
        {
            if (obj == null)
                return;

            SelectedItem = obj;
        }

        public async void ExecuteLoadInventariosCommand(object obj)
        {
            await OnAppearing();
        }

        private async void ExecuteExportFileCommand(Inventario inventario, TipoSistema tipoSistema)
        {
            var arquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), inventario.NomeArquivo);
            if (!Path.HasExtension(arquivo))
                arquivo = $"{arquivo}.txt";

            var names = arquivo.Split('-');
            if (names.Length == 2)
            {
                arquivo = $"{names[0]}-{inventario.DataCriacao.ToString("ddMMyyyyHHmm")}.txt";
            }

            var arquivoString = string.Empty;            

            await foreach (var coleta in stockService.GetColetasAsync(inventario.Id))
            {
                switch (tipoSistema)
                {
                    case TipoSistema.EMSys:
                        if (coleta.Codigo.Length < 20)
                            arquivoString += $"{coleta.Codigo};{coleta.Quantidade}\n";
                        break;
                    case TipoSistema.AutoSystem:
                        {
                            var linhaString = $"{coleta.Codigo.PadLeft(14, '0')}{coleta.Quantidade.ToString().PadLeft(6, '0')}0000000{coleta.Hora.ToString("dd/MM/yyHH:mm:ss")}\n";
                            if (linhaString.Length == 44)
                                arquivoString += linhaString;
                            break;
                        }
                    default:
                        break;
                }
            }
            File.WriteAllText(arquivo, arquivoString);
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Arquivo de inventário",
                File = new ShareFile(arquivo)
            });
        }


        private bool CanExportFile(object arg)
        {
            return _selectedItem != null;
        }

        internal void ExportInventarioFile(Inventario inventario, TipoSistema tipoSistema = TipoSistema.AutoSystem)
        {
            ExecuteExportFileCommand(inventario, tipoSistema);
        }

        public Inventario SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);//OnItemSelected(value);
        }

        public string NomeArquivo { get => nomeArquivo; set => SetProperty(ref nomeArquivo, value); }
        public DateTime DataCriacao { get => dataCriacao; set => SetProperty(ref dataCriacao, value); }
        public Config Config { get; private set; }
    }
}
