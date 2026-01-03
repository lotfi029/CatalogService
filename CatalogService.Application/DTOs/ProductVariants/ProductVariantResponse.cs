using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.ProductVariants;

public sealed record ProductVariantResponse(
    Guid ProductVariantId,
    string Sku,
    ProductVariantsOption VariantAttributes,
    decimal Price,
    string Currency,
    decimal? CompareAtPrice)
{
    private ProductVariantResponse() : this(
        Guid.Empty,
        string.Empty,
        default!,
        0m, 
        string.Empty, 
        null)
    {
        
    }
}