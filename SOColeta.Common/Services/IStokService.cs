using SOColeta.Common.DataModels;
using SOColeta.Common.Models;

namespace SOColeta.Common.Services;

public interface IStokService
{
    Task<InventarioModel?> GetInventario(string serial);
    Task LancarInventario(Guid? inventarioId, long? pessoa);
    Task<IList<Inventario>> BuscarInventarios(string inserted);
    Task<InventarioModel> RegistrarInventario(Inventario inventario);
    Task<Inventario?> FinalizarInventario(InventarioModel model);
}
