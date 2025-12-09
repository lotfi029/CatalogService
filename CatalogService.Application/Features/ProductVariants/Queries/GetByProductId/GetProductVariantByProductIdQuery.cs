using CatalogService.Application.DTOs.ProductVariants;

namespace CatalogService.Application.Features.ProductVariants.Queries.GetByProductId;

public sealed record GetProductVariantByProductIdQuery(Guid Id) : IQuery<List<ProductVariantResponse>>;

public sealed class GetProductVariantByProductIdQueryHandler(
    IProductVariantQueries productVariantQueries,
    ILogger<GetProductVariantByProductIdQueryHandler> logger) : IQueryHandler<GetProductVariantByProductIdQuery, List<ProductVariantResponse>>
{
    public async Task<Result<List<ProductVariantResponse>>> HandleAsync(GetProductVariantByProductIdQuery query, CancellationToken ct = default)
    {
        if (query.Id == Guid.Empty)
            return ProductVariantErrors.InvalidId;

        try
        {
            return await productVariantQueries.GetByProductIdAsync(query.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieve product variant with id: {productVariantId}",
                query.Id);
            return ProductVariantErrors.GetProductVariantByProductId;
        }
    }
}