using Microsoft.EntityFrameworkCore;
using TicketRush.Modules.Catalog.Domain;

namespace TicketRush.Modules.Catalog.Infrastructure;

public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Venue> Venues => Set<Venue>();

    public DbSet<VenueSeat> VenueSeats => Set<VenueSeat>();

    public DbSet<Event> Events => Set<Event>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("catalog");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}