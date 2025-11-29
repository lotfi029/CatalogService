using CatalogService.Application.DTOs.Categories;

namespace CatalogService.Application.Features.Categories.Queries.GetById;

public sealed class GetCategoryByIdQueryHandler(
    ICategoryQueries queries,
    ILogger<GetCategoryByIdQueryHandler> logger) : IQueryHandler<GetCategoryByIdQuery, CategoryDetailedResponse>
{
    public async Task<Result<CategoryDetailedResponse>> HandleAsync(GetCategoryByIdQuery query, CancellationToken ct = default)
    {
        if (query.Id == Guid.Empty)
            return CategoryErrors.InvalidId;
        try
        {
            return await queries.GetByIdAsync(query.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while retrieving category with ID {CategoryId}",
                query.Id);

            return Error.Unexpected("Failed to retrieve category");
        }
    }
}
