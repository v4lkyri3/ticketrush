using TicketRush.Modules.Catalog.Domain;

namespace TicketRush.Modules.Catalog.Tests;

public class VenueTests
{

    List<string> sections = new List<string>()
    {
        "Front","Top","Side"
    };
    string VenueTest = "VenueTest";
    string CityTest = "CityTest";

    [Fact]
    public void CreateWithGrid_creates_sections_x_rows_x_seats_seats()
    {
        var result = Venue.CreateWithGrid(VenueTest, CityTest, sections, 5, 10).Value;

        Assert.Equal(150, result.Seats.Count);
    }

    [Fact]
    public void CreateWithGrid_names_rows_from_A()
    {

        var result = Venue.CreateWithGrid(VenueTest, CityTest, sections, 2, 5).Value;
        var rows = result.Seats.Select(x => x.Row).Distinct().OrderBy(r => r).ToList();
        Assert.Equal(["A", "B"], rows);
    }

    [Fact]
    public void CreateWithGrid_numbers_seats_from_1()
    {
        var result = Venue.CreateWithGrid(VenueTest, CityTest, sections, 2, 10).Value;

        var obj = result.Seats.Where(x => x.Section == "Front" && x.Row == "A").ToList();

        Assert.Equal(1, obj.First().Number);
        Assert.Equal(10, obj.Last().Number);
    }

    [Fact]
    public void All_seats_belong_to_the_venue()
    {
        var result = Venue.CreateWithGrid(VenueTest, CityTest, sections, 2, 10).Value;

        Assert.All(result.Seats, s => Assert.Equal(result.Id, s.VenueId));

    }

    [Fact]
    public void Seat_positions_are_unique()
    {
        var result = Venue.CreateWithGrid(VenueTest, CityTest, sections, 2, 10).Value;

        var seats = result.Seats.Select(X => (X.Section, X.Row, X.Number)).ToList();

        Assert.Equal(seats.Count, seats.Distinct().Count());
    }

    [Fact]
    public void Seats_is_not_backed_by_a_List_the_caller_can_cast_to()
    {
        var venue = Venue.CreateWithGrid(VenueTest, CityTest, sections, 2, 3).Value;

        Assert.False(venue.Seats is List<VenueSeat>);
    }

    [Fact]
    public void Adding_through_a_cast_throws_and_leaves_the_seats_unchanged()
    {
        var venue = Venue.CreateWithGrid(VenueTest, CityTest, sections, 2, 3).Value;
        var before = venue.Seats.Count;
        var existingSeat = venue.Seats.First();

        var attack = (ICollection<VenueSeat>)venue.Seats;

        Assert.Throws<NotSupportedException>(() => attack.Add(existingSeat));
        Assert.Equal(before, venue.Seats.Count);
    }

    [Theory]
    [InlineData(-4)]
    [InlineData(30)]
    [InlineData(0)]
    public void RowNumber_greater_than_26_or_less_than_1(int rowNumber)
    {
        var venue = Venue.CreateWithGrid(VenueTest, CityTest, sections, rowNumber, 3);

        Assert.Equal("venue.rows_invalid", venue.Error.Code);
    }

    [Theory]
    [InlineData(-4)]
    [InlineData(0)]
    public void SeatNumber_greater_than_26_or_less_than_1(int number)
    {
        var venue = Venue.CreateWithGrid(VenueTest, CityTest, sections, 2, number);

        Assert.Equal("venue.seats_invalid", venue.Error.Code);
    }
}