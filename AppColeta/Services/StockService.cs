using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SOColeta.Data;
using SOColeta.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            logger.LogDebug("Buscando inventario aberto...");
            var inventario = await dbContext.Inventarios
                .Where(i => !i.IsFinished)
                .FirstOrDefaultAsync();

            if (inventario == null)
                dbContext.Inventarios.Add(new Inventario());

            if (string.IsNullOrEmpty(coleta.Id))
                coleta.Id = Guid.NewGuid().ToString();

            coleta.Inventario = inventario;
            logger.LogDebug("Verificando coleta já existente...");
            var coletaOld = await dbContext.Coletas
                .Where(x => x.Inventario == inventario)
                .Where(x => x.Codigo == coleta.Codigo)
                .FirstOrDefaultAsync();

            if (coletaOld != null)
            {
                logger.LogDebug("Atualizando coleta...");
                coletaOld.Quantidade = coleta.Quantidade;
                dbContext.Coletas.Update(coletaOld);
            }
            else
            {
                logger.LogDebug("Adicionando coleta...");
                dbContext.Coletas.Add(new Coleta());
            }
            logger.LogDebug("Salvando alterações na coleta existente...");
            await dbContext.SaveChangesAsync();
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
            inventario.DataCriacao = DateTime.Now;
            inventario.NomeArquivo = $"Inventario-{inventario.Id}-{inventario.DataCriacao.ToString("ddMMyyyyHHmm")}";
            dbContext.Inventarios.Update(inventario);
            await dbContext.SaveChangesAsync();
        }

        public async IAsyncEnumerable<Coleta> GetColetasAsync(string id = default)
        {
            logger.LogDebug("Buscando inventario aberto...");
            var query = dbContext.Inventarios.AsQueryable();
            if (string.IsNullOrEmpty(id))
                query = query.Where(i => !i.IsFinished);
            else
                query = query.Where(i => i.Id == id);

            var inventario = await query
                .Include(x => x.ProdutosColetados)
                .FirstOrDefaultAsync();
            if (inventario is null && string.IsNullOrEmpty(id))
            {
                logger.LogInformation("Nenhum inventário aberto localizado!");
                yield break;
            }
            else if (inventario is null)
            {
                logger.LogError("Não foi encontrado inventário com a id solicitada: {0}", id);
                throw new ArgumentNullException($"Não foi encontrado inventário com a id solicitada: {id}");
            }
            if (inventario.ProdutosColetados is null)
            {
                logger.LogError("Não foram encontrados produtos coletados");
                inventario.ProdutosColetados = new List<Coleta>();
            }

            foreach (var coleta in inventario.ProdutosColetados)
                yield return coleta;
        }

        public async IAsyncEnumerable<Inventario> GetFinishedInventarios()
        {
            logger.LogDebug("Buscando inventarios finalizados...");
            var inventarios = await dbContext.Inventarios
                .Where(x => x.IsFinished)
                .ToArrayAsync();

            foreach (var inventario in inventarios)
                yield return inventario;
        }

        public Task<Produto> GetProduto(string barcode)
        {
            return dbContext.Produtos.FirstOrDefaultAsync(produto => produto.Codigo == barcode);
        }

        public async Task<bool> InventarioHasColeta()
        {
            logger.LogDebug("Verificando inventário em aberto e se o mesmo possui coletas...");
            var inventario = await dbContext.Inventarios
                .Where(i => !i.IsFinished)
                .Include(x => x.ProdutosColetados)
                .FirstOrDefaultAsync();

            return inventario.ProdutosColetados.Any();
        }

        public async Task RemoveColeta(Coleta coleta)
        {
            if (coleta == null && string.IsNullOrEmpty(coleta.Id))
                throw new ArgumentNullException("Coleta não pode ser nula");

            if (coleta.Inventario == null && string.IsNullOrEmpty(coleta.InventarioId))
                throw new ArgumentNullException("Inventário nao pode ser nulo ao remover uma coleta.");

            var coletaOld = await dbContext.Coletas
                .Where(c => c.Id == coleta.Id)
                .FirstOrDefaultAsync();

            if (coletaOld is not null)
            {
                dbContext.Coletas.Remove(coletaOld);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
