using SOColeta.Common;
using SOColeta.Common.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCommon();
builder.Services.Configure<Database>(options => builder.Configuration.GetSection("Database").Bind(options));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Run();
