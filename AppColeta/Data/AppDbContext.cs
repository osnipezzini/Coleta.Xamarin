using AppColeta.Models;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace AppColeta.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Coleta> Coletas { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
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
