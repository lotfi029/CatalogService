namespace CatalogService.Application.DTOs.ProductAttributes;

public sealed record ProductAttributeResponse(
    Guid Id,
    string AttributeName,
    string Code,
    bool IsFilterable,
    bool IsSearchable,
    string Value
    )
{
    private ProductAttributeResponse() : this(
        Guid.Empty,
        string.Empty,
        string.Empty,
        false,
        false,
        string.Empty)
    { }
}