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

    public static Error FailedToAddVariantAttribute(int cnt) =>
        Error.BadRequest(
            $"{_code}.{nameof(FailedToAddVariantAttribute)}",
            $"An Error ocurred while adding: '{cnt}' variant attribute definition please try agin");
    public static Error CreateCommandException
        => Error.Unexpected("Error Ocurred while adding new variant attribute definition");
    public static Error CreateBulkCommandException =>
        Error.Unexpected("Error ocurred while adding bulk or variant attribute definition");

}