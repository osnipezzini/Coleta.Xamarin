using SOColeta.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOColeta.Services
{
    public interface IStockService
    {
        Task<string> CreateInventario();
        Task<bool> InventarioHasColeta();
        IAsyncEnumerable<Coleta> GetColetasAsync(string id = default);
        Task AddColeta(Coleta coleta);
        Task RemoveColeta(Coleta coleta);
        Task FinishInventario();
        IAsyncEnumerable<Inventario> GetFinishedInventarios();
        Task<Produto> GetProduto(string barcode);
        Task AddProduto(Produto produto);
    }
}
