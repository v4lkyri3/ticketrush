using TicketRush.SharedKernel;

namespace TicketRush.SharedKernel.Tests;
public class ResultTests
{
    [Fact]
    public void Error_factories_set_the_matching_type()
    {
        var error = Error.Conflict("seat.unavailable","Seat Taken");
        Assert.Equal(ErrorType.Conflict, error.Type);
    }

    [Fact]
    public void Reading_value_of_a_failed_result_throws()
    {
        var result = Result.Failure<int>(Error.Failure("x","boom"));
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void Success_has_no_error()
    {
        var result = Result.Success();
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_exposes_its_error_and_IsFailure_is_true()
    {
        var error = Error.Failure("404'","Test");
        var result = Result.Failure(error);
        Assert.Equal(error,result.Error);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Reading_Value_of_a_successful_result_returns_it()
    {
        var val = "Its Working";
        var result = Result.Success(val);
        Assert.True(result.IsSuccess);
        Assert.Equal(val,result.Value);

    }

    [Fact]
    public void Error_converts_to_a_failed_Result_implicitly()
    {
        var error = Error.NotFound("a","b");
        Result result = error;
        Assert.True(result.IsFailure);
        Assert.Equal(error,result.Error);

    }

    [Fact]
    public void Error_converts_to_a_failed_Result_with_Type_implicitly()
    {
        var error = Error.NotFound("a","b");
        Result<int> result = error;
        Assert.True(result.IsFailure);
        Assert.Equal(error,result.Error);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

}
