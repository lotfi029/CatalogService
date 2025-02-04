namespace Domain.Abstractions;

public record Error(string Code, string Description, int? StatusCode)
{
    public static Error None => new(string.Empty, string.Empty, null);

    public static implicit operator Result(Error error) => Result.Failure(error);
}