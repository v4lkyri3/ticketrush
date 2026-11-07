using TicketRush.SharedKernel;

namespace TicketRush.Modules.Catalog.Domain;

public sealed class Venue : Entity
{
    public string Name { get; private set; } = default!;
    public string City { get; private set; } = default!;

    private List<VenueSeat> _seats = [];

    public IReadOnlyCollection<VenueSeat> Seats => _seats.AsReadOnly();

    private Venue()
    {
        
    }

    public static Result<Venue> CreateWithGrid(string name,string city,IEnumerable<string> sections,int rowsPerSection, int seatsPerRow)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            return Error.Validation("venue.name_required","The Venue name is required");
        }

        if(string.IsNullOrWhiteSpace(city))
        {
            return Error.Validation("venue.city_required","The Venue city is required");
        }

        if(sections is null)
        {
            return Error.Validation("venue.sections_invalid","Atleast 1 section is required");
        }

        if( rowsPerSection<1 || rowsPerSection > 26)
        {
            return Error.Validation("venue.rows_invalid","The number of rows should be between 1 and 26");
        }

        if( seatsPerRow < 1)
        {
            return Error.Validation("venue.seats_invalid","The number of seats should be atleast 1");
        }

        var sectionList = sections.Select(x=>x?.Trim() ?? string.Empty).ToList();

        if(sectionList.Count == 0 
        || sectionList.Any(string.IsNullOrEmpty)
        || sectionList.Distinct(StringComparer.OrdinalIgnoreCase).Count() != sectionList.Count)
        {
            return Error.Validation("venue.sections_invalid","Sections must be non empty, non-blank and unique");
        }

        Venue venue = new Venue()
        {
            Name=name.Trim(),
            City=city.Trim()
        };

        foreach(var section in sectionList)
        {
            for(int i=0;i<rowsPerSection;i++)
            {
                var row = ((char)('A'+i)).ToString();
                for(int j=0;j<seatsPerRow;j++)
                {
                    var seat = new VenueSeat(venue.Id,section,row,j+1);
                    venue._seats.Add(seat);
                }
            }
        }
        return venue;
    }
}