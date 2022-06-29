using SOColeta.Common;
using SOColeta.Common.Models;
using SOColeta.Domain;
using SOColeta.Domain.Services;

using SOCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSOCore();
builder.Services.ConfigureCommon();
builder.Services.ConfigureDomain();
builder.Services.Configure<Database>(options => builder.Configuration.GetSection("Database").Bind(options));
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x => 
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

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
