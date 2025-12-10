using CatalogService.Application.DTOs.ProductCategories;

namespace CatalogService.Application.Features.ProductCategories.Queries.Get;

public sealed record GetProductCategoryByIdQuery(Guid ProductId, Guid CategoryId) : IQuery<ProductCategoryResponse>;

internal sealed class GetProductCategoryByIdQueryHandler(
    IProductCategoryQueries productCategoryQueries,
    ILogger<GetProductCategoryByIdQueryHandler> logger) : IQueryHandler<GetProductCategoryByIdQuery, ProductCategoryResponse>
{
    public async Task<Result<ProductCategoryResponse>> HandleAsync(GetProductCategoryByIdQuery query, CancellationToken ct = default)
    {
        if (query.ProductId == Guid.Empty || query.CategoryId == Guid.Empty)
            return ProductCategoriesErrors.InvalidId;

        try
        {
            return await productCategoryQueries.GetAsync(
                productId: query.ProductId,
                categoryId: query.CategoryId,
                ct: ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieve productCategory with productId: '{productId}' and categoryId: '{categoryId}'",
                query.ProductId, query.CategoryId);
            return ProductCategoriesErrors.GetProductCategory;
        }
    }
}