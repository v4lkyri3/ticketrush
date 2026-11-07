using TicketRush.SharedKernel;

namespace TicketRush.Modules.Catalog.Domain;

public sealed class VenueSeat : Entity
{
    public Guid VenueId {get; private set;}
    public string Section { get; private set; } = default!;
    public string Row { get; private set; } = default!;
    public int Number { get; private set; }

    private VenueSeat()
    {
        
    }

    internal VenueSeat(Guid venueId,string section, string row, int number)
    {
        this.VenueId = venueId;
        this.Section = section;
        this.Row = row;
        this.Number = number;
    }
}