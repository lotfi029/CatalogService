using CatalogService.Application.DTOs.ProductCategories;

namespace CatalogService.Application.Features.ProductCategories.Queries.GetByProductId;

public sealed record GetProductCategoriesByProductIdQuery(Guid ProductId) : IQuery<IEnumerable<ProductCategoryResponse>>;

internal sealed class GetProductCategoriesByProductIdQueryHandler(
    IProductCategoryQueries productCategoryQueries,
    ILogger<GetProductCategoriesByProductIdQueryHandler> logger) : IQueryHandler<GetProductCategoriesByProductIdQuery, IEnumerable<ProductCategoryResponse>>
{
    public async Task<Result<IEnumerable<ProductCategoryResponse>>> HandleAsync(GetProductCategoriesByProductIdQuery query, CancellationToken ct = default)
    {
        if (query.ProductId == Guid.Empty)
            return ProductCategoriesErrors.InvalidId;

        try
        {
            var result = await productCategoryQueries.GetByProductIdAsync(
                productId: query.ProductId,
                ct: ct);

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieve productCategories with productId: '{productId}'",
                query.ProductId);
            return ProductCategoriesErrors.GetProductCategory;
        }
    }
}