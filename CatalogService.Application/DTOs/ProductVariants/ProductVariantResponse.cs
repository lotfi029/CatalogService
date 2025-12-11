using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.ProductVariants;

public sealed record ProductVariantResponse(
    Guid ProductVariantId,
    string Sku,
    ProductVariantsOption VariantAttributes,
    ProductVariantsOption? CustomizationOptions,
    decimal Price,
    string Currency,
    decimal? CompareAtPrice)
{
    private ProductVariantResponse() : this(
        Guid.Empty,
        string.Empty,
        default!,
        null,
        0m, 
        string.Empty, 
        null)
    {
        
    }
}