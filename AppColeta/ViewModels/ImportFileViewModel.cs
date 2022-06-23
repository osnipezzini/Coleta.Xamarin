using Acr.UserDialogs;

using Matcha.BackgroundService;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SOColeta.Models;
using SOColeta.Services;

using SOTech.Mvvm;

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
        private string message;
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
        public string ProgressMessage { get => message; set => SetProperty(ref message, value); }

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
                    OnPropertyChanged(nameof(Filename));
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
            if (fullpath is null)
                return;
            var stream = await fullpath.OpenReadAsync();

            // Registra
            BackgroundAggregatorService.Add(() => new BackgroundImportService(stockService, stream));

            // Inicia
            BackgroundAggregatorService.StartBackgroundService();
            await Shell.Current.DisplayAlert("Importação iniciada", "Sua importação foi iniciada, iremos lhe avisar assim que concluirmos.", "Ok");
        }
    }
}
