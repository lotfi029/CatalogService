using CatalogService.Application.DTOs.Categories;
using CatalogService.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace CatalogService.Application.Features.Categories.Queries;

public sealed record GetCategoryBySlugQuery(string Slug) : IQuery<CategoryDetailedResponse>;

public sealed class GetCategoryBySlugQueryHandler(
    ILogger<GetCategoryBySlugQueryHandler> logger,
    IDbConnectionFactory connectionFactory) : IQueryHandler<GetCategoryBySlugQuery, CategoryDetailedResponse>
{
    public async Task<Result<CategoryDetailedResponse>> HandleAsync(GetCategoryBySlugQuery query, CancellationToken ct = default)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();

            var sql = """
                SELECT c.id as Id, c.name as Name, c.slug as Slug, c.parent_id as ParentId, c.level as Level, c.description as Description, c.path as Path
                FROM public.categories c
                WHERE c.slug = @slug
                """;

            var response = await connection.QuerySingleOrDefaultAsync<CategoryDetailedResponse>(sql, new { slug = query.Slug });

            if (response is null)
                return CategoryErrors.SlugNotFound(query.Slug);

            return response;

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