using SOColeta.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOColeta.Services
{
    public class MockDataStore : IDataStore<Coleta>
    {
        static readonly List<Coleta> coletas = new List<Coleta>();

        public MockDataStore()
        {
        }

        public async Task<bool> AddItemAsync(Coleta item)
        {
            coletas.Add(item);

            return await Task.FromResult(true);
        }
        public int Count { get =>coletas.Count; }
        public async Task<bool> UpdateItemAsync(Coleta item)
        {
            var oldItem = coletas.Where((Coleta arg) => arg.Id == item.Id).FirstOrDefault();
            coletas.Remove(oldItem);
            coletas.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = coletas.Where((Coleta arg) => arg.Id == id).FirstOrDefault();
            coletas.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Coleta> GetItemAsync(string id)
        {
            return await Task.FromResult(coletas.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Coleta>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(coletas);
        }
    }
}