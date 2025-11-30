using CatalogService.Application.DTOs.Categories;

namespace CatalogService.Application.Features.Categories.Queries.Tree;

internal sealed class GetCategoryTreeQueryHandler(
    ICategoryQueries categoryQueries,
    ILogger<GetCategoryTreeQueryHandler> logger) : IQueryHandler<GetCategoryTreeQuery, IEnumerable<CategoryResponse>>
{
    public async Task<Result<IEnumerable<CategoryResponse>>> HandleAsync(GetCategoryTreeQuery query, CancellationToken ct = default)
    {
        try
        {
            return await categoryQueries.GetTreeAsync(query.ParentId, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Occurred while retriving categories");
            return Error.Unexpected("Error Occurred while retriving categories");
        }
    }
}