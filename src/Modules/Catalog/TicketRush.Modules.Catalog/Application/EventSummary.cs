namespace TicketRush.Modules.Catalog.Application;

public sealed record EventSummary(
    Guid Id, string Title, DateTime StartsAtUtc, string VenueName, string City, decimal BasePrice
);

