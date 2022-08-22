using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SOColeta.Data;
using SOColeta.Exceptions;
using SOColeta.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task AddColeta(Coleta coleta, bool? replaceOld = null)
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
                if (!replaceOld.HasValue)
                    throw new ColetaConflictException("Coleta já existe!");

                logger.LogDebug("Atualizando coleta...");
                if (replaceOld.Value)
                    coletaOld.Quantidade = coleta.Quantidade;
                else
                    coletaOld.Quantidade += coleta.Quantidade;
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

        public Task SetInventarioName(Inventario inventario, string name)
        {
            inventario.NomeArquivo = name;
            dbContext.Entry(inventario).State = EntityState.Modified;
            dbContext.Inventarios.Update(inventario);
            return dbContext.SaveChangesAsync();
        }
    }
}
