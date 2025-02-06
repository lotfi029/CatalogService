using Domain.Enums;

namespace Domain.Abstractions;

public record Error(string Code, string Details, ErrorType ErrorType)
{
    public static Error None => new(string.Empty, string.Empty, ErrorType.Failure);

    public static implicit operator Result(Error error) => Result.Failure(error);

    public string Code { get; } = Code;
    public string Details { get; } = Details;
    public ErrorType ErrorType { get; } = ErrorType;

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Conflict(string code, string description)
        => new(code, description, ErrorType.Conflict);

    public static Error UnAutherization(string code, string description)
        => new(code, description, ErrorType.UnAutherization);

    public static Error Validation(string code, string description)
        => new(code, description, ErrorType.Validation);

    public static Error BadRequest(string code, string description)
        => new(code, description, ErrorType.BadRequest);
}


