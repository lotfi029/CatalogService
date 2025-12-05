using CatalogService.Application.DTOs.Products;

namespace CatalogService.Application.Features.Products.Queries.Get;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<ProductDetailedResponse>;

internal sealed class GetProductByIdQueryHandler(
    IProductQueries productQueries,
    ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdQuery, ProductDetailedResponse>
{
    public async Task<Result<ProductDetailedResponse>> HandleAsync(GetProductByIdQuery query, CancellationToken ct = default)
    {
        if (query.Id == Guid.Empty)
            return ProductErrors.InvalidId;

        try
        {
            return await productQueries.GetAsync(query.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieve product with id: {productId}",
                query.Id);

            return ProductErrors.GetProductById;
        }
    }
}