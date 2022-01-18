using SOColeta.Models;

using SOTech.Core.Services;

using SQLite;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SOColeta.Services
{
    public class SqliteDB : IDatabase
    {
        private static SQLiteAsyncConnection db;
        private readonly ILogger logger;

        public int Count
        {
            get
            {
                Init().RunSynchronously();
                return db.ExecuteScalarAsync<int>("SELECT count(*) FROM Coleta").Result;
            }
        }
        public SqliteDB(ILogger logger)
        {
            this.logger = logger;
        }

        static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "socoleta.db");
            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTablesAsync<Coleta, Inventario, Produto>();
        }

        public async Task<bool> AddInventarioAsync(Inventario inventario)
        {
            await Init();
            var result = await db.InsertAsync(inventario);
            return (result > 0);
        }

        public async Task<bool> AddColetaAsync(Coleta coleta)
        {
            await Init();
            var result = await db.InsertAsync(coleta);
            return (result > 0);
        }

        public async Task<bool> UpdateColetaAsync(Coleta coleta)
        {
            await Init();
            var result = await db.UpdateAsync(coleta);
            return (result > 0);
        }

        public async Task<bool> DeleteColetaAsync(int id)
        {
            await Init();
            var result = await db.DeleteAsync<Coleta>(id);
            return result > 0;
        }

        public async Task<Coleta> GetColetaAsync(int id)
        {
            await Init();
            var result = await db.GetAsync<Coleta>(id);
            return result;
        }

        public async IAsyncEnumerable<Coleta> GetColetasAsync(int inventarioId = 0)
        {
            await Init();

            var queryIds = new StringBuilder()
                .Append("select Id from Coleta");

            if (inventarioId > 0)
                queryIds.AppendFormat(" where InventarioId = {0}", inventarioId);

            var ids = await db.ExecuteScalarAsync<int[]>("select Id from Coleta");

            foreach (var id in ids)
            {
                var query = db.Table<Coleta>();
                if (inventarioId > 0)
                    query.Where(x => x.InventarioId == inventarioId);
                yield return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<int> CountColetasByInventarioAsync(int inventarioId)
        {
            await Init();

            var query = new StringBuilder()
                .Append("select count(*) from Coleta")
                .AppendFormat(" where InventarioId = {0}", inventarioId);
            var sql = query.ToString();
            logger.Debug(sql);

            var result = await db.ExecuteScalarAsync<int>(sql);
            return result;
        }

        public async Task<Produto> GetProdutoAsync(string codigo)
        {
            await Init();
            var result = await db.Table<Produto>()
                .Where(p => p.Codigo == codigo)
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> AddOrUpdateProdutoAsync(Produto produto)
        {
            await Init();
            int result;

            if (produto.Id > 0)
                result = await db.UpdateAsync(produto);
            else
            {
                var produtoOld = await GetProdutoAsync(produto.Codigo);
                if (produtoOld is null)
                    result = await db.InsertAsync(produto);
                else
                    result = await db.UpdateAsync(produto);
            }

            return result > 0;                
        }

        public async IAsyncEnumerable<Inventario> GetInventariosAsync()
        {
            await Init();

            var queryIds = new StringBuilder()
                .Append("select Id from Inventario");

            var ids = await db.ExecuteScalarAsync<int[]>(queryIds.ToString());

            foreach (var id in ids)
            {
                var query = db.Table<Inventario>();
                yield return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<Inventario> GetInventarioAsync(int id)
        {
            await Init();

            var result = await db.Table<Inventario>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
