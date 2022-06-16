using SOColeta.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOColeta.Services
{
    public interface IStockService
    {
        Task<Inventario> CreateInventario();
        Task<Inventario> CreateInventario(Inventario inventario);
        Task<bool> InventarioHasColeta();
        Task<Inventario> GetOpenedInventario();
        IAsyncEnumerable<Coleta> GetColetasAsync(Guid? guid);
        Task AddColeta(Coleta coleta);
        Task RemoveColeta(Coleta coleta);
        Task FinishInventario();
        IAsyncEnumerable<Inventario> GetFinishedInventarios();
        Task<Product> GetProduto(string barcode);
        Task AddProduto(Product produto);
        Task ExportInventario(Inventario inventario);
        Task SyncData();
    }
}
