namespace CatalogService.Application.DTOs.ProductAttributes;

public sealed record ProductAttributeResponse(
    Guid AttributeId,
    string AttributeName,
    string AttributeCode,
    bool IsFilterable,
    bool IsSearchable,
    string AttributeValue
    )
{
    private ProductAttributeResponse() : this(
        AttributeId: Guid.Empty,
        AttributeName: string.Empty,
        AttributeCode: string.Empty,
        IsFilterable: false,
        IsSearchable: false,
        AttributeValue: string.Empty)
    { }
}