using CatalogService.Application.DTOs.Categories;
using CatalogService.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace CatalogService.Application.Features.Categories.Queries.GetById;

public sealed class GetCategoryByIdQueryHandler(
    IDbConnectionFactory factory,
    ILogger<GetCategoryByIdQueryHandler> logger) : IQueryHandler<GetCategoryByIdQuery, CategoryDetailedResponse>
{
    public async Task<Result<CategoryDetailedResponse>> HandleAsync(GetCategoryByIdQuery query, CancellationToken ct = default)
    {
        if (query.Id == Guid.Empty)
            return CategoryErrors.InvalidId;
        try
        {
            using var connection = factory.CreateConnection();

            var sql = """
                SELECT 
                    c.id as Id, 
                    c.name as Name, 
                    c.slug as Slug, 
                    c.parent_id as ParentId, 
                    c.level as Level, 
                    c.description as Description, 
                    c.path as Path
                FROM public.categories c
                WHERE c.id = @id 
                    AND c.is_deleted = false
                """;
            var category = await connection.QuerySingleOrDefaultAsync<CategoryDetailedResponse>(
                sql,
                new { id = query.Id });

            if (category is null)
                return CategoryErrors.NotFound(query.Id);

            return category;
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
