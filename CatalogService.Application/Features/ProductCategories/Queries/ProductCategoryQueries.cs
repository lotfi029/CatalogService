using CatalogService.Application.DTOs.ProductCategories;

namespace CatalogService.Application.Features.ProductCategories.Queries;

public interface IProductCategoryQueries
{
    Task<IEnumerable<ProductCategoryResponse>> GetByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task<Result<ProductCategoryResponse>> GetAsync(Guid productId, Guid categoryId, CancellationToken ct = default);
}
