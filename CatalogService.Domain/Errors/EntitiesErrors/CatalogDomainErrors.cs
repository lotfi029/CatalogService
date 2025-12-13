namespace CatalogService.Domain.Errors.EntitiesErrors;

public static partial class DomainErrors
{
    public static Error Null(string code) 
        => Error.BadRequest(code, "value cannot be empty or white space");
    public static Error NullNumber(string code) 
        => Error.BadRequest(code, "value cannot be less than or equal 0");
}