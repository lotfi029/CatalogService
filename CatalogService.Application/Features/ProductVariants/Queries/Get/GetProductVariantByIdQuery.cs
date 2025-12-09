using CatalogService.Application.DTOs.ProductVariants;

namespace CatalogService.Application.Features.ProductVariants.Queries.Get;

public sealed record GetProductVariantByIdQuery(Guid Id) : IQuery<ProductVariantResponse>;

public sealed class GetProductVariantByIdQueryHandler(
    IProductVariantQueries productVariantQueries,
    ILogger<GetProductVariantByIdQueryHandler> logger) : IQueryHandler<GetProductVariantByIdQuery, ProductVariantResponse>
{
    public async Task<Result<ProductVariantResponse>> HandleAsync(GetProductVariantByIdQuery query, CancellationToken ct = default)
    {
        if (query.Id == Guid.Empty)
            return ProductVariantErrors.InvalidId;

        try
        {
            return await productVariantQueries.GetAsync(query.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieve product variant with id: {productVariantId}",
                query.Id);
            return ProductVariantErrors.GetProductVariantById;
        }
    }
}