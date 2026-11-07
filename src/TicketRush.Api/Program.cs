using TicketRush.Api;
using TicketRush.Modules.Catalog;
using TicketRush.Modules.Catalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCatalogModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    await DevDataSeeder.SeedAsync(db);
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

