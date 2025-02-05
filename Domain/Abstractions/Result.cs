using System.ComponentModel;

namespace Domain.Abstractions;
public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && Error.None == error)
            throw new InvalidOperationException("Invalid error state");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }


    public static Result Success() => 
        new(true, Error.None);
    public static Result Failure(Error error)
        => new(false, error);
    public static Result<TValue> Success<TValue>(TValue value) =>
    new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error)
        => new(default, false, error);
}
public class Result<TValue>(TValue? value, bool isSuccess, Error error) : Result(isSuccess, error)
{
    private readonly TValue? _value = value;

    public TValue? Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Cannot access value in a failure state");
}
