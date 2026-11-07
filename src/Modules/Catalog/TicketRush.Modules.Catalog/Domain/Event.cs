using TicketRush.SharedKernel;

namespace TicketRush.Modules.Catalog.Domain;

public enum EventStatus
{
    Draft, OnSale, SoldOut, Cancelled
}

public class Event : Entity
{
    public Guid VenueId {get; private set;}
    public string Title {get; private set;}
    public string Description {get; private set;}
    public DateTime StartsAtUtc {get; private set;} 
    public decimal BasePrice {get; private set;}
    public EventStatus Status {get; private set;}

    private Event()
    {
        
    }
    public static Result<Event> Create(Guid venueId, string title, string description, DateTime startsAtUtc, decimal basePrice, DateTime nowUtc)
    {
        if(string.IsNullOrWhiteSpace(title))
        {
            var error = Error.Validation("event.title_required","Even Title should not be empty!");
            return Result.Failure<Event>(error);
        }
        
        if(startsAtUtc < nowUtc)
        {
            return Error.Validation("event.start_in_past","Start Date shouldnt be in future.");
        }

        if(basePrice <=0)
        {
            return Error.Validation("event.price_invalid","Invalid Price Value.");
        }
        var EventFinal = new Event{VenueId = venueId, Title = title.Trim() , Description = description, StartsAtUtc = startsAtUtc, BasePrice = basePrice, Status = EventStatus.Draft};
        return Result.Success<Event>(EventFinal);
        
    }

    public Result Publish(DateTime nowUTC)
    {
        if(Status != EventStatus.Draft)
        {
            return Error.Conflict("event.not_draft","The event should be in draft to publish.");
        }

        if(nowUTC >= StartsAtUtc)
        {
            return Error.Conflict("event.start_in_past","The event has already started.");           
        }

        Status = EventStatus.OnSale;
        return Result.Success();
    }
 }