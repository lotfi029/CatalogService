using CatalogService.Application.DTOs.ProductVariants;

namespace CatalogService.Application.Features.ProductVariants.Queries;

public interface IProductVariantQueries
{
    Task<Result<ProductVariantResponse>> GetAsync(Guid productVariantId, CancellationToken ct = default);
    Task<Result<List<ProductVariantResponse>>> GetByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task<Result<List<ProductVariantResponse>>> GetSkuAsync(string sku, CancellationToken ct = default);
}
