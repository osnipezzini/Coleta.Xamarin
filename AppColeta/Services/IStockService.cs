using SOColeta.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOColeta.Services
{
    public interface IStockService
    {
        Task<Inventario> CreateInventario();
        Task<bool> InventarioHasColeta();
        Task<Inventario> GetOpenedInventario();
        IAsyncEnumerable<Coleta> GetColetasAsync(string id, int? max = null);
        Task AddColeta(Coleta coleta, bool? replaceOld = null);
        Task RemoveColeta(Coleta coleta);
        Task FinishInventario(string inventarioId);
        IAsyncEnumerable<Inventario> GetFinishedInventarios();
        Task<Produto> GetProduto(string barcode);
        Task AddProduto(Produto produto);
        Task SetInventarioName(Inventario inventario, string name);
        Task<Coleta> GetColetaAsync(string coletaId);
        Task<Inventario> GetInventario(string inventarioId);
        Task<Coleta> GetAndDeleteColetaAsync(string coletaId);
        IAsyncEnumerable<Inventario> GetOpenedInventarios();
        Task DeleteInventario(Inventario inventario);
        Task ReopenInventario(Inventario inventario);
    }
}
