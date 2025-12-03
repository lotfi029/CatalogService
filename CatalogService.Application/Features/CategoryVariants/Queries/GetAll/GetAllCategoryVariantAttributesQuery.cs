using CatalogService.Application.DTOs.CategoryVariantAttributes;

namespace CatalogService.Application.Features.CategoryVariants.Queries.GetAll;

public sealed record GetAllCategoryVariantAttributesQuery(Guid CategoryId) : IQuery<IEnumerable<CategoryVariantAttributeDetailedResponse>>;

public sealed class GetAllCategoryVariantAttributesQueryHandler(
    ILogger<GetAllCategoryVariantAttributesQueryHandler> logger,
    ICategoryVariantAttributeQueries variantQueries) : IQueryHandler<GetAllCategoryVariantAttributesQuery, IEnumerable<CategoryVariantAttributeDetailedResponse>>
{
    public async Task<Result<IEnumerable<CategoryVariantAttributeDetailedResponse>>> HandleAsync(GetAllCategoryVariantAttributesQuery query, CancellationToken ct = default)
    {
        try
        {
            return await variantQueries.GetByCategoryIdAsync(query.CategoryId, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieving category variant attributes to category id: {categoryId}",
                query.CategoryId);
                         
            return CategoryVariantAttributeErrors.GetAllCategoryVariantAttribute;
        }
    }
}