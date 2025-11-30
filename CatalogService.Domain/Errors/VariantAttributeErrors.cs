namespace CatalogService.Domain.Errors;

public sealed class VariantAttributeErrors
{
    private const string _code = "VariantAttributes";
    public static Error NotFound(Guid id) =>
        Error.BadRequest(
            $"{_code}.{nameof(NotFound)}",
            $"Variant attribute with id: '{id}' does not exist");

    public static Error InvalidId
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidId)}",
            $"Please enter a valid id");

    public static Error CodeAlreadyExist(string code) =>
        Error.BadRequest(
            $"{_code}.{nameof(CodeAlreadyExist)}",
            $"Variant attribute with code: '{code}' already exist");
}