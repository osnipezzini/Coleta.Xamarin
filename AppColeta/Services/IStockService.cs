﻿using SOColeta.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOColeta.Services
{
    public interface IStockService
    {
        Task<Inventario> CreateInventario();
        Task<bool> InventarioHasColeta();
        Task<Inventario> GetOpenedInventario();
        IAsyncEnumerable<Coleta> GetColetasAsync(string id = default);
        Task AddColeta(Coleta coleta);
        Task RemoveColeta(Coleta coleta);
        Task FinishInventario();
        IAsyncEnumerable<Inventario> GetFinishedInventarios();
        Task<Produto> GetProduto(string barcode);
        Task AddProduto(Produto produto);
        Task SetInventarioName(Inventario inventario, string name);
    }
}
