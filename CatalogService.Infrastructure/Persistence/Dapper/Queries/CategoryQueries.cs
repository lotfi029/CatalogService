using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Domain.Errors;
using Dapper;
namespace CatalogService.Infrastructure.Persistence.Dapper.Queries;

public sealed class CategoryQueries(
    IDbConnectionFactory connectionFactory) : ICategoryQueries
{
    public async Task<Result<CategoryDetailedResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        using var connection = connectionFactory.CreateConnection();

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
            new CommandDefinition(sql, new { id }, cancellationToken: ct));

        if (category is null)
            return CategoryErrors.NotFound(id);

        return category;
    }

    public async Task<Result<CategoryDetailedResponse>> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        using var connection = connectionFactory.CreateConnection();

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
                WHERE c.slug = @slug 
                    AND c.is_deleted = false
                """;
        var category = await connection.QuerySingleOrDefaultAsync<CategoryDetailedResponse>(
            new CommandDefinition(sql, new { slug }, cancellationToken: ct));

        if (category is null)
            return CategoryErrors.SlugNotFound(slug);

        return category;
    }

    public async Task<Result<IEnumerable<CategoryResponse>>> GetTreeAsync(Guid? parentId, CancellationToken ct = default)
    {
        using var connection = connectionFactory.CreateConnection();

        IEnumerable<CategoryResponse> response;
        if (parentId.HasValue && parentId.Value != Guid.Empty)
        {
            var sql = """
                    WITH RECURSIVE tree AS (
                        SELECT c.*
                        from public.categories c
                        WHERE c.id = @id
                            AND is_deleted = false
                        UNION ALL
                        SELECT c.*
                        FROM public.categories c
                        INNER JOIN tree pc 
                            ON c.parent_id = pc.id
                        WHERE c.is_deleted = false 
                    )
                    SELECT 
                        id as Id, 
                        name as Name, 
                        slug as Slug, 
                        parent_id as ParentId, 
                        level as Level, 
                        path as Path
                    FROM tree
                    order by level
                    """;

            response = await connection.QueryAsync<CategoryResponse>(
                new CommandDefinition(sql, new { id = parentId}, cancellationToken: ct));
        }
        else
        {
            var sql = """
                    SELECT 
                        c.id as Id, c.name as Name, c.slug as Slug, c.parent_id as ParentId, c.level as Level, c.path as Path
                    FROM public.categories c
                    WHERE c.is_deleted = false
                    """;
            response = await connection.QueryAsync<CategoryResponse>(
                new CommandDefinition(sql, cancellationToken: ct));
        }


        if (response is null)
            return Result.Success(Enumerable.Empty<CategoryResponse>());

        return Result.Success(response);
    }
}
