using SOColeta.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace SOColeta.Domain.Services;

public interface IMigrationService
{
    void Migrate();
    Task MigrateAsync();
}
public class MigrationService : IMigrationService
{
    private readonly AppDbContext dbContext;

    public MigrationService(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Migrate()
    {
        dbContext.Database.Migrate();
    }

    public async Task MigrateAsync()
    {
        await dbContext.Database.MigrateAsync();
    }
}