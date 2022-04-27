using Microsoft.Extensions.DependencyInjection;

using SOColeta.Common.Services;
using SOColeta.Domain.Data;
using SOColeta.Domain.Services;

namespace SOColeta.Domain;

public static class DIExtensions
{
    public static void ConfigureDomain(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<AppDbContext>();
        serviceCollection.AddScoped<IMigrationService, MigrationService>();
        serviceCollection.AddScoped<IColetaService, ColetaService>();
        serviceCollection.AddScoped<IStokService, StokService>();
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
