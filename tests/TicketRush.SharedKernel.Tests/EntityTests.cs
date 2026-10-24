using TicketRush.SharedKernel;

namespace TicketRush.SharedKernel.Tests;

public class EntityTests
{
public sealed class TestEntity : Entity;

    [Fact]
    public void New_entity_gets_a_non_empty_id()
    {
        var entity = new TestEntity();
        Assert.NotEqual(Guid.Empty,entity.Id);
    }

    [Fact]
    public void Two_entities_get_different_ids()
    {
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();
        Assert.NotEqual(entity1.Id,entity2.Id);
    }
}