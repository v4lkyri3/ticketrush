using System.Text;
using TicketRush.Modules.Catalog.Domain;

namespace TicketRush.Modules.Catalog.Tests;

public class EventTests
{
    
    private static readonly DateTime Now = new DateTime(2026,10,25,12,0,0,DateTimeKind.Utc);

    private static Event NewDraft() => Event.Create(Guid.NewGuid(),"Guns'n'Roses","Musical Rock Concert",Now.AddDays(10),1200,Now).Value;

    [Fact]
    public void Create_with_valid_data_returns_a_draft_event()
    {
        var result = Event.Create(Guid.NewGuid(),"Test","Test Description",Now.AddDays(5),1000,Now);

        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Draft,result.Value.Status);
        Assert.Equal("Test",result.Value.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_rejects_a_blank_title(string title)
    {
        var result = Event.Create(Guid.NewGuid(),title,"Description",Now.AddDays(10),500,Now);
        
        Assert.True(result.IsFailure);
        Assert.Equal("event.title_required",result.Error.Code);
    }

    [Fact]
    public void Create_rejects_a_start_time_in_the_past()
    {
        var result = Event.Create(Guid.NewGuid(),"Test","Description",Now.AddDays(-10),500,Now);

        Assert.True(result.IsFailure);
        Assert.Equal("event.start_in_past",result.Error.Code);

    }

    
    [Fact]
    public void Create_accepts_a_start_time_same_as_current_time()
    {
        var result = Event.Create(Guid.NewGuid(),"Test","Description",Now,500,Now);

        Assert.True(result.IsSuccess);
        Assert.Equal(Now,result.Value.StartsAtUtc);

    }

    [Theory]
    [InlineData(-100)]
    [InlineData(0)]
    public void Create_rejects_zero_or_negative_price(decimal price)
    {
        var result = Event.Create(Guid.NewGuid(),"Test","Description",Now.AddDays(10),price,Now);

        Assert.True(result.IsFailure);
        Assert.Equal("event.price_invalid",result.Error.Code);

    }

    [Fact]
    public void Create_trims_the_title()
    {
        var result = Event.Create(Guid.NewGuid()," Rock Night!  ","Description",Now.AddDays(10),500,Now);

        Assert.Equal("Rock Night!",result.Value.Title);

    }

    [Fact]
    public void Publish_moves_a_draft_to_OnSale()
    {
        var ev = NewDraft();
        var result = ev.Publish(Now);

        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.OnSale,ev.Status);
    }

    [Fact]
    public void Publish_twice_fails_with_a_conflict()
    {
        var ev = NewDraft();
        var publishOne = ev.Publish(Now);
        var publishTwo = ev.Publish(Now);

        Assert.True(publishOne.IsSuccess);
        Assert.True(publishTwo.IsFailure);
        Assert.Equal("event.not_draft",publishTwo.Error.Code);
        Assert.Equal(EventStatus.OnSale,ev.Status);
    }

    [Fact]
    public void Publish_fails_if_the_event_already_started()
    {
        var ev = NewDraft();

        var result = ev.Publish(Now.AddDays(20));
        Assert.True(result.IsFailure);
        Assert.Equal("event.start_in_past",result.Error.Code);

    }
}