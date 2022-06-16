using Microsoft.EntityFrameworkCore;
using SOColeta.Common.Models;
using Xamarin.Forms;

namespace SOColeta.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Coleta> Coletas { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Product> Produtos { get; set; }
        public AppDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = DependencyService.Get<IDBPath>().GetDbPath();
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
