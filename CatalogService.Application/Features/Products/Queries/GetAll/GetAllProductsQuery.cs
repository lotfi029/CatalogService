using CatalogService.Application.DTOs.Products;

namespace CatalogService.Application.Features.Products.Queries.GetAll;

public sealed record GetAllProductsQuery : IQuery<IEnumerable<ProductResponse>>;

public sealed class GetAllProductsQueryHandler(
    IProductQueries productQueries,
    ILogger<GetAllProductsQueryHandler> logger) : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{
    public async Task<Result<IEnumerable<ProductResponse>>> HandleAsync(GetAllProductsQuery query, CancellationToken ct = default)
    {
        try
        {
            return await productQueries.GetAllAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error ocurred while retrieve products");
            return ProductErrors.GetAllProduct;
        }
    }
}