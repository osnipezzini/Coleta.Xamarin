﻿using Microsoft.Extensions.Logging;

using Acr.UserDialogs;

using SOColeta.Models;
using SOColeta.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using SOColeta.Data;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace SOColeta.ViewModels
{
    internal class ImportFileViewModel : ViewModelBase
    {
        private string filename;
        private FileResult fullpath;
        private readonly AppDbContext dbContext;
        private readonly ILogger<ImportFileViewModel> logger;
        private string busyMessage;
        private bool cancelImport = false; 

        public Command ChooseFileCommand { get; }
        public Command StartImportCommand { get; }
        public Command StopImportCommand { get; }
        public ImportFileViewModel(AppDbContext dbContext, ILogger<ImportFileViewModel> logger)
        {
            Title = "Importar arquivo";
            ChooseFileCommand = new Command(async () => await PickAndShow());
            StartImportCommand = new Command(async () => await ImportFile());
            StopImportCommand = new Command(() => cancelImport = true);
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public string Filename { get => filename; set => SetProperty(ref filename, value); }
        public string BusyMessage { get => busyMessage; set => SetProperty(ref busyMessage, value); }

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
            IsBusy = true;
            var cts = new CancellationTokenSource();
            var stream = await fullpath.OpenReadAsync();
            var readerStream = new StreamReader(stream);
            var contents = await readerStream.ReadToEndAsync();
            string[] file = contents.Split('\n');
            int processedLine = 1;
            BusyMessage = "Iniciando importação de produtos...";
            using (var loading = UserDialogs.Instance.Loading("Iniciando importação de produtos...", 
                maskType: MaskType.Gradient, show:true, cancelText: "Cancelar", onCancel: cts.Cancel))
                foreach (string line in file)
                {
                    if (cancelImport || cts.IsCancellationRequested)
                        break;
                    loading.Title = BusyMessage = $"Processando linha {processedLine} / {file.Length} ...";
                    loading.PercentComplete = (processedLine / file.Length) * 100;

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

                            if (await dbContext.Produtos.AnyAsync(p => p.Codigo == reader[0].Trim()))

                            dbContext.Produtos.Add(new Produto
                            {
                                Codigo = reader[0].Trim(),
                                Nome = reader[1].Trim(),
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
            await dbContext.SaveChangesAsync(cts.Token);
            IsBusy = false;
            await Shell.Current.DisplayAlert("Produtos importados", $"Foram importados {file.Length} produtos.", "Ok");
        }
    }
}
