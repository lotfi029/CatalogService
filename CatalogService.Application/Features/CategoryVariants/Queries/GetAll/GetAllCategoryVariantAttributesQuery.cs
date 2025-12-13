using CatalogService.Application.DTOs.CategoryVariantAttributes;
using Mapster;

namespace CatalogService.Application.Features.CategoryVariants.Queries.GetAll;

public sealed record GetAllCategoryVariantAttributesQuery(Guid CategoryId) : IQuery<IEnumerable<CategoryVariantAttributeDetailedResponse>>;

public sealed class GetAllCategoryVariantAttributesQueryHandler(
    ILogger<GetAllCategoryVariantAttributesQueryHandler> logger,
    ICategoryVariantAttributeQueries variantQueries,
    ICategoryVariantAttributeRepository variantRepository) : IQueryHandler<GetAllCategoryVariantAttributesQuery, IEnumerable<CategoryVariantAttributeDetailedResponse>>
{
    public async Task<Result<IEnumerable<CategoryVariantAttributeDetailedResponse>>> HandleAsync(GetAllCategoryVariantAttributesQuery query, CancellationToken ct = default)
    {
        try
        {
            if (await variantRepository.GetByCategoryIdAsync(query.CategoryId, ct) is not { } categoryVariant)
                return Result.Success(Enumerable.Empty<CategoryVariantAttributeDetailedResponse>());

            var response = categoryVariant.Adapt<IEnumerable<CategoryVariantAttributeDetailedResponse>>();
            return Result.Success(response);
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