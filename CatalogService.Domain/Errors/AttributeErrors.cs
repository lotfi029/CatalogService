namespace CatalogService.Domain.Errors;

public class AttributeErrors
{
    private const string _code = "attributes";
    public static Error InvalidId
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidId)}",
            $"Please enter a valid id");
    public static Error InvalidCode
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidCode)}",
            $"Please enter a valid code");
    
    public static Error InvalidOptinsType
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidOptinsType)}",
            $"Please enter a valid options type");
    public static Error NotFound(Guid id)
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}",
            $"Attribute with id: {id} cannot be found");
    
    public static Error CodeNotFound(string code)
        => Error.NotFound(
            $"{_code}.{nameof(CodeNotFound)}",
            $"Attribute with code: {code} cannot be found");
    public static Error DuplicatedCode(string code)
        => Error.Conflict(
            $"{_code}.{nameof(DuplicatedCode)}",
            $"Attirbute with code {code} is aleary exist");
    public static Error InvalidUpdateOptions
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidUpdateOptions)}",
            "this attribute doesnot have option to update it");
    public static Error CreateAttribute
        => Error.Unexpected("Failed to creating new attribute");
    public static Error UpdateAttributeOptions
        => Error.Unexpected("Failed to update attribute options");
    public static Error UpdateAttributeDetails
        => Error.Unexpected("Failed to update attribute details");
    public static Error DeleteAttribute
        => Error.Unexpected("Failed to delete attribute");
    public static Error ActivateAttribute
        => Error.Unexpected("Failed to activate attribute");
    public static Error DeactivateAttribute
        => Error.Unexpected("Failed to deactivate attribute");

    public static Error GetAttributeById
        => Error.Unexpected("Failed to get attribute by id");
    public static Error GetAllAttribute
        => Error.Unexpected("Failed to get attributes");
    public static Error GetAttributeByCode
        => Error.Unexpected("Failed to get attribute by code");
    public static Error GetAttributeByType
        => Error.Unexpected("Failed to get attributes by type");

}
