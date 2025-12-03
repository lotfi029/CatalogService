using CatalogService.Application.DTOs.CategoryVariantAttributes;

namespace CatalogService.Application.Features.CategoryVariants.Queries;

public interface ICategoryVariantAttributeQueries
{
    Task<Result<IEnumerable<CategoryVariantAttributeDetailedResponse>>> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct = default);
    Task<Result<CategoryVariantAttributeDetailedResponse>> Getsync(Guid categoryId, Guid variantId, CancellationToken ct = default);
}