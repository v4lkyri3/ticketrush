using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketRush.Modules.Catalog.Infrastructure;

namespace TicketRush.Modules.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(
            config.GetConnectionString("Default"),
            npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory","catalog")
        ));

        return services;
    }
}