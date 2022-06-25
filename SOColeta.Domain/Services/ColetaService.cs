using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using SOColeta.Common.DataModels;
using SOColeta.Common.Models;
using SOColeta.Common.Services;
using SOColeta.Domain.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOColeta.Domain.Services
{
    public class ColetaService : IColetaService
    {
        private readonly ILogger<ColetaService> _logger;
        private readonly AppDbContext _dbContext;
        private readonly IMapper mapper;

        public ColetaService(ILogger<ColetaService> logger, AppDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<Coleta?> AddColeta(ColetaModel model)
        {
            _logger.LogDebug(JsonConvert.SerializeObject(model));

            var product = await _dbContext.Products
                .Where(p => p.Barcode == model.Barcode)
                .FirstOrDefaultAsync();

            if (product == null)
                _logger.LogDebug("Produto não encontrado");

            var coleta = new Coleta(model.Barcode, model.Quantidade, model.HoraColeta, model.Inventario);

            if (product is not null)
            {
                mapper.Map(model, product);
                product.Barcode = model.Barcode;

                _dbContext.Products.Update(product);
            }
            else
            {
                product = new Product();
                mapper.Map(model, product);
                product.Barcode = model.Barcode;
                _dbContext.Products.Add(product);
            }

            coleta.Produto = product;
            coleta.ProdutoId = product.Id;

            var inventario = await _dbContext.Inventarios
                .FirstOrDefaultAsync(i => i.Guid == model.Inventario);

            if (inventario is null)
                return null;

            coleta.Inventario = inventario;
            coleta.InventarioId = inventario.Id;
            inventario.ProdutosColetados = null;

            var coleta_old = await _dbContext.Coletas
                .Include(p => p.Produto)
                .Include(i => i.Inventario)
                .Where(c => c.Codigo == model.Barcode)
                .Where(c => c.InventarioGuid == model.Inventario)
                .FirstOrDefaultAsync();

            if (coleta_old is not null)
            {
                coleta_old.Quantidade += model.Quantidade;
                coleta_old.HoraColeta = model.HoraColeta;
                _dbContext.Coletas.Update(coleta_old);
                coleta = coleta_old;
            }
            else
                _dbContext.Coletas.Add(coleta);

            await _dbContext.SaveChangesAsync();

            return coleta;
        }

        public async Task<ColetaModel?> GetColeta(string codigo, Guid? inventarioGuid)
        {
            var coleta = await _dbContext.Coletas
            .Where(c => c.Codigo == codigo && c.InventarioGuid == inventarioGuid)
            .Select(x => mapper.Map<ColetaModel>(x))
            .FirstOrDefaultAsync();

            return coleta;
        }
    }
}
