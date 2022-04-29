using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SOColeta.Data;
using SOColeta.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace SOColeta.Services
{
    public class StockService : IStockService
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<StockService> logger;
        public StockService(AppDbContext dbContext, ILogger<StockService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public async Task AddColeta(Coleta coleta)
        {
            logger.LogDebug("Verificando coleta já existente...");
            var coletaOld = await dbContext.Coletas
                .Where(x => x.InventarioId == coleta.InventarioId)
                .Where(x => x.Codigo == coleta.Codigo)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(coleta.Id))
                coleta.Id = Guid.NewGuid().ToString();

            coleta.Hora = DateTime.Now;

            if (coletaOld != null)
            {
                var update = await Shell.Current
                    .DisplayActionSheet("Já existe uma coleta desse produto nesse inventário, o que deseja fazer ?", "Cancelar", null, "Somar", "Substituir");
                if (update == "Somar")
                    coletaOld.Quantidade += coleta.Quantidade;
                else if (update == "Substituir")
                    coletaOld.Quantidade = coleta.Quantidade;
                else
                {
                    logger.LogDebug("Coleta cancelada!");
                    return;
                }
                logger.LogDebug("Atualizando coleta...");
                coletaOld.Hora = coleta.Hora;
                dbContext.Coletas.Update(coletaOld);
            }
            else
            {
                logger.LogDebug("Adicionando coleta...");
                dbContext.Coletas.Add(coleta);
            }
            logger.LogDebug("Salvando alterações na coleta existente...");
            await dbContext.SaveChangesAsync();
        }

        public async Task<Inventario> CreateInventario()
        {
            var id = Guid.NewGuid().ToString();
            var inventario = new Inventario
            {
                Id = id,
                DataCriacao = DateTime.Now,
                NomeArquivo = $"Inventario-{DateTime.Now.ToString("ddMMyyyyHHmm")}.txt",
                IsFinished = false
            };
            dbContext.Inventarios.Add(inventario);
            await dbContext.SaveChangesAsync();
            return inventario;
        }

        public Task AddProduto(Produto produto)
        {
            dbContext.Produtos.Add(produto);
            return dbContext.SaveChangesAsync();
        }

        public async Task FinishInventario()
        {
            logger.LogDebug("Buscando inventario aberto...");
            var inventario = await dbContext.Inventarios
                .Where(i => !i.IsFinished)
                .FirstOrDefaultAsync();

            inventario.IsFinished = true;
            dbContext.Inventarios.Update(inventario);
            await dbContext.SaveChangesAsync();
        }

        public async IAsyncEnumerable<Coleta> GetColetasAsync(string id = default)
        {
            Debug.WriteLine("Criando query de busca");
            var query = dbContext.Inventarios.AsQueryable();
            if (string.IsNullOrEmpty(id))
                query = query.Where(i => !i.IsFinished);
            else
                query = query.Where(i => i.Id == id);
            Debug.WriteLine("Buscando coletas");
            var coletas = await query
                .Include(x => x.ProdutosColetados)
                .Select(x => x.ProdutosColetados)
                .FirstOrDefaultAsync();

            if (coletas is null && string.IsNullOrEmpty(id))
            {
                Debug.WriteLine("Não foram encontrados coletas");
                yield break;
            }
            else if (coletas is null)
            {
                logger.LogError($"Não foram encontradas coletas para o inventário: {id}");
                throw new ArgumentOutOfRangeException($"Não foram encontradas coletas para o inventário: {id}");
            }

            foreach (var coleta in coletas)
                yield return coleta;
        }

        public async IAsyncEnumerable<Inventario> GetFinishedInventarios()
        {
            logger.LogDebug("Buscando inventarios finalizados...");
            var inventarios = dbContext.Inventarios
                .Where(x => x.IsFinished)
                .ToArrayAsync();

            foreach (var inventario in await inventarios)
                yield return inventario;
        }

        public Task<Produto> GetProduto(string barcode)
        {
            return dbContext.Produtos.FirstOrDefaultAsync(produto => produto.Codigo == barcode);
        }

        public async Task<bool> InventarioHasColeta()
        {
            logger.LogDebug("Verificando inventário em aberto e se o mesmo possui coletas...");
            return await dbContext.Inventarios
                .Include(x => x.ProdutosColetados)
                .Where(i => !i.IsFinished && i.ProdutosColetados.Any())
                .AnyAsync();
        }

        public async Task RemoveColeta(Coleta coleta)
        {
            if (coleta == null && string.IsNullOrEmpty(coleta.Id))
                throw new ArgumentNullException("Coleta não pode ser nula");

            if (coleta.Inventario == null && string.IsNullOrEmpty(coleta.InventarioId))
                throw new ArgumentNullException("Inventário não pode ser nulo ao remover uma coleta.");

            var coletaOld = await dbContext.Coletas
                .Where(c => c.Id == coleta.Id)
                .FirstOrDefaultAsync();

            if (coletaOld is not null)
            {
                dbContext.Coletas.Remove(coletaOld);
                await dbContext.SaveChangesAsync();
            }
        }

        public Task<Inventario> GetOpenedInventario()
        {
            return dbContext.Inventarios.FirstOrDefaultAsync(x => !x.IsFinished);
        }
        private async Task ExportAutoSystem(Inventario inventario)
        {
            var arquivo = string.Empty;
            var arquivoString = string.Empty;
            var tipoInventario = await Shell
                .Current
                .DisplayActionSheet("Selecione o modelo do coletor", "Cancelar", null, Enum.GetNames(typeof(TipoASColeta)));

            if (!Enum.TryParse<TipoASColeta>(tipoInventario, out var tipoSistema))
                return;

            switch (tipoSistema)
            {
                case TipoASColeta.CipherLab:
                    {
                        var nomeArquivo = $"Inventario_Opticon_{inventario.DataCriacao.ToString("ddMMyyyyHHmm")}.txt";
                        arquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nomeArquivo);
                        await foreach (var coleta in GetColetasAsync(inventario.Id))
                        {
                            var quantidade = Convert.ToInt32(coleta.Quantidade).ToString().PadLeft(6, '0');
                            var linhaString = $"{coleta.Codigo.PadLeft(14, '0')}{quantidade}0000000{coleta.Hora.ToString("dd/MM/yyHH:mm:ss")}\n";
                            if (linhaString.Length == 44)
                                arquivoString += linhaString;
                        }
                        break;
                    }
                case TipoASColeta.Opticon:
                    {
                        var nomeArquivo = $"Inventario_CipherLab_{inventario.DataCriacao.ToString("ddMMyyyyHHmm")}.txt";
                        arquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nomeArquivo);
                        await foreach (var coleta in GetColetasAsync(inventario.Id))
                        {
                            var quantidade = Regex.Replace(coleta.Quantidade.ToString("N3"), @"[^0-9]", string.Empty).PadLeft(9, '0');
                            var linhaString = $"{coleta.Codigo.PadLeft(14, '0')}{quantidade}0000000{coleta.Hora.ToString("dd/MM/yyHH:mm:ss")}\n";
                            if (linhaString.Length == 47)
                                arquivoString += linhaString;
                        }
                        break;
                    }
                default:
                    break;
            }
            await ExportFile(arquivo, arquivoString);
        }
        private Task ExportFile(string filename, string content)
        {
            File.WriteAllText(filename, content);
            return Share.RequestAsync(new ShareFileRequest
            {
                Title = "Arquivo de inventário",
                File = new ShareFile(filename)
            });
        }
        private async Task ExportEmsys(Inventario inventario)
        {
            var arquivo = string.Empty;
            var arquivoString = string.Empty;
            await foreach (var coleta in GetColetasAsync(inventario.Id))
                if (coleta.Codigo.Length < 20)
                    arquivoString += $"{coleta.Codigo};{coleta.Quantidade}\n";

            await ExportFile(arquivo, arquivoString);
        }
        public async Task ExportInventario(Inventario inventario)
        {
            var tipoInventario = await Shell
                .Current
                .DisplayActionSheet("Selecione o sistema para exportação", "Cancelar", null, Enum.GetNames(typeof(TipoSistema)));

            if (!Enum.TryParse<TipoSistema>(tipoInventario, out var tipoSistema))
                return;

            switch (tipoSistema)
            {
                case TipoSistema.EMSys:
                    await ExportEmsys(inventario);
                    break;
                case TipoSistema.AutoSystem:
                    await ExportAutoSystem(inventario);
                    break;
                default:
                    break;
            }
        }
    }
}
