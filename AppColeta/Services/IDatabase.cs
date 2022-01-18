using SOColeta.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOColeta.Services
{
    public interface IDatabase
    {
        Task<bool> AddColetaAsync(Coleta coleta);
        Task<bool> UpdateColetaAsync(Coleta coleta);
        Task<bool> DeleteColetaAsync(int id);
        Task<Coleta> GetColetaAsync(int id);
        IAsyncEnumerable<Coleta> GetColetasAsync(int inventarioId = 0);
        Task<bool> AddInventarioAsync(Inventario inventario);
        Task<int> CountColetasByInventarioAsync(int inventarioId);
        Task<Produto> GetProdutoAsync(string codigo);
        Task<bool> AddOrUpdateProdutoAsync(Produto produto);
        IAsyncEnumerable<Inventario> GetInventariosAsync();
        Task<Inventario> GetInventarioAsync(int id);
    }
}
