using Microsoft.Extensions.Logging;

using SOColeta.Models;
using SOColeta.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    internal class ImportFileViewModel : ViewModelBase
    {
        private string filename;
        private FileResult fullpath;
        private readonly IStockService stockService;
        private readonly ILogger<ImportFileViewModel> logger;

        public Command ChooseFileCommand { get; }
        public Command StartImportCommand { get; }
        public ImportFileViewModel(IStockService stockService, ILogger<ImportFileViewModel> logger)
        {
            Title = "Importar arquivo";
            ChooseFileCommand = new Command(async () => await PickAndShow());
            StartImportCommand = new Command(async () => await ImportFile());
            this.stockService = stockService;
            this.logger = logger;
        }
        public string Filename { get => filename; set => SetProperty(ref filename, value); }

        private async Task<bool> PickAndShow()
        {

            var options = new PickOptions
            {
                FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new string[] { "text/txt", "text/csv", "text/text"} },
                    { DevicePlatform.iOS, new string[] { "public.image" } },
                    { DevicePlatform.UWP, new string[] { ".jpg", ".png" } },
                }),
                PickerTitle = "Selecione o arquivo para ser importado"
            };
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    Filename = result.FileName;
                    fullpath = result;

                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao importar arquivo de inventario!");
            }
            return false;
        }

        private async Task ImportFile()
        {
            var stream = await fullpath.OpenReadAsync();
            var readerStream = new StreamReader(stream);
            var contents = await readerStream.ReadToEndAsync();
            string[] file = contents.Split('\n');
            foreach (string line in file)
            {
                if (!string.IsNullOrWhiteSpace(line) && (line.Contains(";") || line.Contains(",")))
                {
                    try
                    {
                        char splitter = line.Contains(";") ? ';' : ',';
                        var reader = line.Replace("\r", "").Split(splitter);
                        double custo = 0;
                        double venda = 0;

                        if (reader.Length >= 4)
                            double.TryParse(reader[3].Replace('.', ','), out custo);
                        if (reader.Length >= 3)
                            double.TryParse(reader[2].Replace('.', ','), out venda);

                        await stockService.AddProduto(new Produto
                        {
                            Codigo = reader[0],
                            Nome = reader[1],
                            PrecoVenda = venda,
                            PrecoCusto = custo,
                        });
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Não foi possível validar a linha selecionada.");
                    }

                }
            }

            await Shell.Current.DisplayAlert("Produtos importados", $"Foram importados {file.Length} produtos.", "Ok");
        }
    }
}
