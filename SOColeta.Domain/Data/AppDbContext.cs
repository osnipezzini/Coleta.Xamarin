using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Npgsql;

using SOColeta.Common.Models;

using SOCore.Services;

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace SOColeta.Domain.Data;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> logger;
    private readonly ISOHelper helper;
    private readonly Database database;
    #region Construtores
    public AppDbContext([NotNull] DbContextOptions dbContextOptions, ILogger<AppDbContext> logger,
        ISOHelper helper, IOptions<Database> options)
        : base(dbContextOptions)
    {
        this.logger = logger;
        this.helper = helper;
        database = options.Value;
    }
    #endregion
    #region Propriedades
    public DbSet<Coleta> Coletas => Set<Coleta>();
    public DbSet<Inventario> Inventarios => Set<Inventario>();
    public DbSet<Product> Products => Set<Product>();
    #endregion
    #region Métodos
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var conn = new NpgsqlConnection(database.ConnectionString);
        conn.Open();
        var version = conn.PostgreSqlVersion;

        var db = database.Clone();
        optionsBuilder.UseNpgsql(db.ConnectionString, opt =>
        {
            opt.SetPostgresVersion(version);
            opt.MigrationsAssembly("SOColeta.Server");
        });
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.LogTo(msg =>
        {
            var printMsg = msg
            .Replace(Environment.NewLine, string.Empty)
            .Replace(@"\r", string.Empty).Trim();
            var message = Regex.Replace(printMsg, @"\s+", " ");
            logger.LogDebug(printMsg);
        });
        if (helper.IsDebug)
            optionsBuilder.EnableSensitiveDataLogging();
    }
    #endregion
}
