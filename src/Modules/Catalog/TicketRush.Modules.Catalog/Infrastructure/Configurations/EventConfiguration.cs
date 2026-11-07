using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketRush.Modules.Catalog.Domain;

namespace TicketRush.Modules.Catalog.Infrastructure.Configurations;

internal sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> b)
    {
        b.ToTable("events");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        b.Property(x => x.Description).HasMaxLength(4000).IsRequired();
        b.Property(x => x.BasePrice).HasPrecision(12,2);
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);

        b.HasOne<Venue>()
            .WithMany()
            .HasForeignKey(x=> x.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex( x => new { x.Status,x.StartsAtUtc});

    }
}