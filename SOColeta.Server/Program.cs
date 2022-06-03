using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SOColeta.Common;
using SOColeta.Common.Models;
using SOColeta.Domain;
using SOColeta.Domain.Data;
using SOColeta.Domain.Services;

using SOCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSOCore();
builder.Services.ConfigureCommon();
builder.Services.ConfigureDomain();
builder.Services.Configure<Database>(options => builder.Configuration.GetSection("Database").Bind(options));
builder.Services.AddControllers();

var app = builder.Build();

var migrationService = app.Services.CreateScope().ServiceProvider.GetRequiredService<IMigrationService>();
migrationService.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseDomain();
app.MapControllers();

app.Run();
