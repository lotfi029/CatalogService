using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Application.DTOs.ProductCategories;
using CatalogService.Application.DTOs.ProductVariants;

namespace CatalogService.Application.DTOs.Products;

public sealed record ProductDetailedResponse(
    Guid Id,
    string Name,
    string? Description,
    string Status,
    Guid VendorId,
    DateTime CreatedAt,
    DateTime? LastUpdateAt,
    bool IsActive,
    List<ProductCategoryResponse>? ProductCategories,
    List<ProductVariantResponse>? ProductVariants,
    List<ProductAttributeResponse>? ProductAttributes)
{
    private ProductDetailedResponse() : this(
        Id: Guid.Empty,
        Name: string.Empty,
        Description: null,
        Status: string.Empty,
        VendorId: Guid.Empty,
        CreatedAt: default,
        LastUpdateAt: null,
        IsActive: true,
        null,
        null,
        null)
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