using CatalogService.Application.DTOs.ProductVariants;

namespace CatalogService.Application.Features.ProductVariants.Queries.GetBySku;

public sealed record GetProductVariantBySkuQuery(string Sku) : IQuery<List<ProductVariantResponse>>;

public sealed class GetProductVariantBySkuQueryHandler(
    IProductVariantQueries productVariantQueries,
    ILogger<GetProductVariantBySkuQueryHandler> logger) : IQueryHandler<GetProductVariantBySkuQuery, List<ProductVariantResponse>>
{
    public async Task<Result<List<ProductVariantResponse>>> HandleAsync(GetProductVariantBySkuQuery query, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query.Sku))
            return ProductVariantErrors.InvalidId;

        try
        {
            return await productVariantQueries.GetSkuAsync(query.Sku, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieve product variant with sku: {sku}",
                query.Sku);
            return ProductVariantErrors.GetProductVariantBySku;
        }
    }
}