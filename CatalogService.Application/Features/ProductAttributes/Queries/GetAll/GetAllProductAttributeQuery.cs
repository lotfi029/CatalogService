using CatalogService.Application.DTOs.ProductAttributes;

namespace CatalogService.Application.Features.ProductAttributes.Queries.GetAll;

public sealed record GetAllProductAttributeQuery(Guid ProductId) : IQuery<IEnumerable<ProductAttributeResponse>>;

internal sealed class GetAllProductVariantQueryHandler(
    IProductAttributeQueries productAttributeQueries,
    ILogger<GetAllProductVariantQueryHandler> logger) : IQueryHandler<GetAllProductAttributeQuery, IEnumerable<ProductAttributeResponse>>
{
    public async Task<Result<IEnumerable<ProductAttributeResponse>>> HandleAsync(GetAllProductAttributeQuery query, CancellationToken ct = default)
    {
        if (Guid.Empty == query.ProductId)
            return ProductAttributeErrors.InvalidId;

        try
        {
            var result = await productAttributeQueries.GetAllByProductIdAsync(query.ProductId, ct);

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error while retrieve attributes owned by product: '{productid}'",
                query.ProductId);
            return ProductAttributeErrors.GetAllProductAttribute;
        }
    }
}