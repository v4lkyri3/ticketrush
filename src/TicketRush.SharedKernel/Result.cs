using System;

namespace TicketRush.SharedKernel;

public enum ErrorType
{
    Validation, NotFound, Conflict, PaymentFailed, Failure
}

public sealed record Error(string Code, string Description, ErrorType Type)
{
    public static Error Conflict(string code, string description) => new Error(code, description, ErrorType.Conflict);
    public static Error Validation(string code, string description) => new Error(code, description, ErrorType.Validation);
    public static Error NotFound(string code, string description) => new Error(code, description, ErrorType.NotFound);
    public static Error PaymentFailed(string code, string description) => new Error(code, description, ErrorType.PaymentFailed);
    public static Error Failure(string code, string description) => new Error(code, description, ErrorType.Failure);
}

public class Result
{

    protected Result(bool isSuccess,Error? error)
    {
        if(isSuccess && error is not null)
            throw new InvalidOperationException("A Successful Result cannot carry on error.");
        if(!isSuccess && error is null) 
            throw new InvalidOperationException("A failed result must have an error.");

        IsSuccess = isSuccess;
        Error = error;    
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error {get;}

    public static Result Success() => new (true,null);
    public static Result Failure(Error error) => new(false,error);
    public static Result<T> Success<T>(T value) => new (value,true,null);
    public static Result<T> Failure<T>(Error error) => new (default,false,error);

    public static implicit operator Result(Error error) => Failure(error);
}

public sealed class Result<T> : Result
{
    private readonly T? _value;
    internal Result(T? value,bool isSuccess,Error? error) : base(isSuccess,error) => _value=value;

    public T Value => IsSuccess? _value! : throw new InvalidOperationException("Cannot read value of a Failed Result");

    public static implicit operator Result<T>(T value) => new (value,true,null);
    public static implicit operator Result<T>(Error error) => new (default,false,error);

}