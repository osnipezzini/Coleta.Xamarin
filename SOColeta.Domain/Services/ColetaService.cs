using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using SOColeta.Common.DataModels;
using SOColeta.Common.Models;
using SOColeta.Common.Services;
using SOColeta.Domain.Data;

namespace SOColeta.Domain.Services
{
    public class ColetaService : IColetaService
    {
        private readonly ILogger<ColetaService> _logger;
        private readonly AppDbContext _dbContext;
        private readonly IMapper mapper;
        private readonly IStokService stokService;

        public ColetaService(ILogger<ColetaService> logger, AppDbContext dbContext, IMapper mapper, IStokService stokService)
        {
            _logger = logger;
            _dbContext = dbContext;
            this.mapper = mapper;
            this.stokService = stokService;
        }
        public async Task AddColeta(ColetaModel model)
        {
            _logger.LogDebug(JsonConvert.SerializeObject(model));

            var product = await stokService.GetProduct(model.Barcode, null);

            if (product == null)
                _logger.LogDebug("Produto não encontrado");

            var coleta = new Coleta(model.Barcode, model.Quantidade, model.HoraColeta, model.Inventario);

            coleta.ProdutoId = product.Id;

            var inventario = await _dbContext.Inventarios
                .FirstOrDefaultAsync(i => i.Guid == model.Inventario);

            if (inventario is null)
                return;

            coleta.Inventario = inventario;
            coleta.InventarioId = inventario.Id;
            coleta.InventarioGuid = inventario.Guid;
            coleta.ProdutoId = product.Grid;
            coleta.HoraColeta = DateTime.UtcNow;
            coleta.Guid = Guid.NewGuid();
            inventario.ProdutosColetados = null;

            var coleta_old = await _dbContext.Coletas
                .Include(i => i.Inventario)
                .Where(c => c.Codigo == model.Barcode)
                .Where(c => c.InventarioGuid == model.Inventario)
                .FirstOrDefaultAsync();

            if (coleta_old is not null)
            {
                coleta_old.Quantidade += model.Quantidade;
                coleta_old.HoraColeta = DateTime.UtcNow;
                _dbContext.Coletas.Update(coleta_old);
                coleta = coleta_old;
            }
            else
                _dbContext.Coletas.Add(coleta);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Coleta?>> GetColeta(Guid? inventarioGuid)
        {
            var coleta = await _dbContext.Coletas
            .Where(c => c.InventarioGuid == inventarioGuid)
            .Select(x => mapper.Map<Coleta>(x))
            .ToListAsync();

            return coleta;
        }
    }
}
