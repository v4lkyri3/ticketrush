using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketRush.Modules.Catalog.Domain;

namespace TicketRush.Modules.Catalog.Infrastructure.Configurations;

internal sealed class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> b)
    {
        b.ToTable("venues");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.Name).HasMaxLength(200).IsRequired();
        b.Property(x => x.City).HasMaxLength(100).IsRequired();

        b.HasMany(x => x.Seats)
            .WithOne()
            .HasForeignKey(s => s.VenueId)
            .OnDelete(DeleteBehavior.Cascade);
            
        b.Navigation(x => x.Seats).UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}