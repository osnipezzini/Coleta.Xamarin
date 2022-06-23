using Matcha.BackgroundService;

using Plugin.LocalNotification;

using SOColeta.Models;

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SOColeta.Services
{
    internal class BackgroundImportService : IPeriodicTask
    {
        private readonly IStockService stockService;
        private readonly string filePath;
        private readonly Stream fileStream;

        public TimeSpan Interval => TimeSpan.FromSeconds(15);
        public BackgroundImportService(IStockService stockService, Stream fileStream)
        {
            this.stockService = stockService;
            this.fileStream = fileStream;
        }
        public async Task<bool> StartJob()
        {
            var readerStream = new StreamReader(fileStream);
            var contents = await readerStream.ReadToEndAsync();
            string[] file = contents.Split('\n');
            var ignoredLines = 0;
            var lineNumber = 0;
            foreach (string line in file)
            {
                lineNumber++;
                if (!string.IsNullOrWhiteSpace(line) && (line.Contains(";") || line.Contains(",")))
                {
                    try
                    {
                        char splitter = line.Contains(";") ? ';' : ',';
                        var reader = line.Replace("\r", "").Split(splitter);
                        var barCode = reader[0].Trim();
                        double custo = 0;
                        double venda = 0;

                        if (string.IsNullOrEmpty(barCode))
                        {
                            ignoredLines++;
                            continue;
                        }

                        if (reader.Length >= 4)
                            double.TryParse(reader[3].Replace('.', ','), out custo);
                        if (reader.Length >= 3)
                            double.TryParse(reader[2].Replace('.', ','), out venda);

                        await stockService.AddProduto(new Produto
                        {
                            Codigo = barCode,
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
            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "Importação finalizada!",
                Description = $"Foram importados {file.Length - ignoredLines} produtos."
            };
            await NotificationCenter.Current.Show(notification);
            return false;
        }
    }
}
