using Dapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using Npgsql;

using SOColeta.Common.DataModels;
using SOColeta.Common.Models;
using SOColeta.Common.Services;
using SOColeta.Domain.Data;

using System.Data;

namespace SOColeta.Domain.Services;

public class StokService : IStokService
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<StokService> logger;
    private readonly IDbConnection connection;

    public StokService(AppDbContext dbContext, ILogger<StokService> logger, IOptions<Database> options)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        connection = new NpgsqlConnection(options.Value.ConnectionString);
    }
    public async Task<IList<Inventario>> BuscarInventarios(string inserted)
    {
        var query = dbContext.Inventarios
                        .Include(i => i.ProdutosColetados)
                        .Where(i => i.IsValid);
        if (!string.IsNullOrEmpty(inserted))
            query = query.Where(c => c.IsInserted == bool.Parse(inserted));
        var inventarios = await query
                .ToArrayAsync();

        return inventarios;
    }

    public async Task<Inventario?> FinalizarInventario(InventarioModel model)
    {
        logger.LogDebug(JsonConvert.SerializeObject(model));
        var inventario = await dbContext.Inventarios
            .Where(i => i.Guid == model.Guid)
            .FirstOrDefaultAsync();

        if (inventario is null)
            return null;

        inventario.IsValid = true;

        dbContext.Inventarios.Update(inventario);
        await dbContext.SaveChangesAsync();
        return inventario;
    }

    public async Task<InventarioModel?> GetInventario(string serial)
    {
        var inventario = await dbContext.Inventarios
                        .Where(i => !i.IsValid)
                        .Where(i => i.Device == serial)
                        .Select(i => new InventarioModel(i.Guid, i.DataCriacao))
                        .FirstOrDefaultAsync();
        if (inventario != null)
        {
            var coletas = await dbContext.Coletas
                    .Include(c => c.Produto)
                .Where(c => c.InventarioId == inventario.Id)
                .Select(c => new ColetaModel(c))
                .ToListAsync();
            coletas.ForEach(c => inventario.Coletas.Add(c));
        }

        return inventario;
    }

    public async Task LancarInventario(Guid? inventarioId, long? pessoa)
    {
        var query = "INSERT INTO coletor_web(inventario, produto, quantidade, ts, codigo)" +
                    " VALUES (@Inventario, @Produto, @Quantidade, @Ts, @Codigo);";
        var inventario = await dbContext.Inventarios
            .Include(c => c.ProdutosColetados)
            .ThenInclude(p => p.Produto)
                .Where(i => i.Guid == inventarioId)
                .FirstOrDefaultAsync();

        if (inventario is null)
            return;

        foreach (var coleta in inventario.ProdutosColetados)
        {
            if (coleta is null || coleta.Produto is null || coleta.Produto.Grid <= 0)
                continue;

            _ = await connection.ExecuteAsync(query, new
            {
                Inventario = coleta.InventarioId,
                Produto = coleta.Produto.Grid,
                coleta.Quantidade,
                Ts = coleta.HoraColeta,
                coleta.Codigo
            });
        }

        inventario.IsInserted = true;
        dbContext.Inventarios.Update(inventario);
        await dbContext.SaveChangesAsync();
    }

    public async Task<InventarioModel> RegistrarInventario(Inventario inventario)
    {
        logger.LogDebug(JsonConvert.SerializeObject(inventario));

        if (inventario.Guid is null)
            inventario.Guid = Guid.NewGuid();

        try
        {
            dbContext.Inventarios.Add(inventario);
            await dbContext.SaveChangesAsync();
            var model = new InventarioModel(inventario.Guid, inventario.DataCriacao);
            return model;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
