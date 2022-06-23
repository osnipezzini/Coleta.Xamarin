using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using SOColeta.Common.Services;
using SOColeta.Domain.Data;
using SOColeta.Domain.Services;

namespace SOColeta.Domain;

public static class DIExtensions
{
    public static IServiceCollection ConfigureDomain(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<AppDbContext>();
        serviceCollection.AddScoped<IMigrationService, MigrationService>();
        serviceCollection.AddScoped<IColetaService, ColetaService>();
        serviceCollection.AddScoped<IStokService, StokService>();
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "SOColeta",
                Description = "API de inventário",
                Contact = new OpenApiContact
                {
                    Name = "Osni Pezzini Junior",
                    Url = new Uri("mailto:osni@sotech.xyz")
                }
            });
        });

        return serviceCollection;
    }

    public static void UseDomain(this WebApplication host)
    {
        // Configure the HTTP request pipeline.
        if (host.Environment.IsDevelopment())
        {
            host.UseSwagger();
            host.UseSwaggerUI();
        }
    }
}
