using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketRush.Modules.Catalog.Domain;

namespace TicketRush.Modules.Catalog.Infrastructure.Configurations;

internal sealed class VenueSeatConfiguration : IEntityTypeConfiguration<VenueSeat>
{
    public void Configure(EntityTypeBuilder<VenueSeat> b)
    {
        b.ToTable("venue_seats");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.Section).HasMaxLength(50).IsRequired();
        b.Property(x => x.Row).HasMaxLength(10).IsRequired();

        b.HasIndex(x => new {x.VenueId, x.Section,x.Row,x.Number}).IsUnique();


    }
}