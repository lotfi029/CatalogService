namespace CatalogService.Application.DTOs.Products;

public sealed record ProductDetailedResponse(
    Guid Id,
    string Name,
    string? Description,
    string Status,
    Guid VendorId,
    DateTime CreatedAt,
    DateTime? LastUpdateAt,
    bool IsActive)
{
    private ProductDetailedResponse() : this(
        Id: Guid.Empty,
        Name: string.Empty,
        Description: null,
        Status: string.Empty,
        VendorId: Guid.Empty,
        CreatedAt: default,
        LastUpdateAt: null,
        IsActive: true)
    { }
}
/*
- categoryResponse 
------ Id, Name, Slug, IsPrimary
- variantResponse 
------ Id, VariantAttribute, custome
- attributeResponse
------ Id, Code, Value, IsFilterable, IsSearchable
*/