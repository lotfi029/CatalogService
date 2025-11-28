using Microsoft.Extensions.Logging;

namespace CatalogService.Application.Features.Categories.Queries.Tree;

public sealed record GetAllCategoryQuery(Guid? ParentId) : IQuery<IEnumerable<CategoryResponse>>;

public sealed class GetAllCategoryQueryHandler(
    IDbConnectionFactory connectionFactory,
    ILogger<GetAllCategoryQueryHandler> logger) : IQueryHandler<GetAllCategoryQuery, IEnumerable<CategoryResponse>>
{
    public async Task<Result<IEnumerable<CategoryResponse>>> HandleAsync(GetAllCategoryQuery query, CancellationToken ct = default)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();

            IEnumerable<CategoryResponse> response;
            if (query.ParentId.HasValue && query.ParentId.Value != Guid.Empty)
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
                            ON c.id = pc.parent_id
                        WHERE c.is_deleted = false 
                    )
                    SELECT 
                        id as Id, name as Name, slug as Slug, parent_id as ParentId, level as Level, path as Path
                    FROM tree
                    order by level
                    """;

                response = await connection.QueryAsync<CategoryResponse>(
                    sql, 
                    new { id = query.ParentId });

            }
            else
            {
                var sql = """
                    SELECT 
                        c.id as Id, c.name as Name, c.slug as Slug, c.parent_id as ParentId, c.level as Level, c.path as Path
                    FROM public.categories c
                    """;
                response = await connection.QueryAsync<CategoryResponse>(sql);
            }


            if (response is null)
                return Result.Success(Enumerable.Empty<CategoryResponse>());

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error Occurred while retriving categories"
                );

            return Error.Unexpected("Error Occurred while retriving categories");
        }
    }
}

public record CategoryResponse(
    Guid Id,
    string Name,
    string Slug,
    Guid ParentId,
    short Level,
    string? Path);