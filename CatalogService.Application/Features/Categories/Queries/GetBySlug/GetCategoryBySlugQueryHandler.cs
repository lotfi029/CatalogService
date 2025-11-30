using CatalogService.Application.DTOs.Categories;

namespace CatalogService.Application.Features.Categories.Queries.GetBySlug;

internal sealed class GetCategoryBySlugQueryHandler(
    ILogger<GetCategoryBySlugQueryHandler> logger,
    ICategoryQueries queries) : IQueryHandler<GetCategoryBySlugQuery, CategoryDetailedResponse>
{
    public async Task<Result<CategoryDetailedResponse>> HandleAsync(GetCategoryBySlugQuery query, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query.Slug))
            return CategoryErrors.InvalidSlug;
        try
        {
            return await queries.GetBySlugAsync(query.Slug, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error Occurred while retrive category with slug: '{slug}'",
                query.Slug);
            return Error.Unexpected("Error Occurred while retrive category by slug");
        }
    }
}