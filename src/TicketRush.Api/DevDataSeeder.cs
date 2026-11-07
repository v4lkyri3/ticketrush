using Microsoft.EntityFrameworkCore;
using TicketRush.Modules.Catalog.Domain;
using TicketRush.Modules.Catalog.Infrastructure;

namespace TicketRush.Api;

internal static class DevDataSeeder
{
    public static async Task SeedAsync(CatalogDbContext db)
    {
        if(await db.Venues.AnyAsync())
            return;
        
        var now = DateTime.UtcNow;

        var arena = Venue.CreateWithGrid("Grand Arena", "Mumbai", ["VIP", "A", "B"], rowsPerSection: 5, seatsPerRow: 10).Value;

        var theatre =  Venue.CreateWithGrid("City Theatre", "Bengaluru", ["Stalls", "Balcony"], rowsPerSection: 4, seatsPerRow: 8).Value;

        db.Venues.AddRange(arena,theatre);

        var events = new[]
        {
            Event.Create(arena.Id,"Rock Night Live","An evening of live rock",now.AddDays(21),1500m,now).Value,
            Event.Create(arena.Id,"Stand up Special","Unlimited Comedy",now.AddDays(35),1000m,now).Value,
            Event.Create(theatre.Id,"The Inception","Nested Dreams",now.AddDays(14),500m,now).Value,
        };

        foreach(var ev in events)
        {
            var published = ev.Publish(now);
            if(published.IsFailure)
            {
                throw new InvalidOperationException($"Seeding has errors! + {published.Error!.Description}");
            }
        }

        db.Events.AddRange(events);

        await db.SaveChangesAsync();

    }
}