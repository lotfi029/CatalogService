using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.DTOs.CategoryVariantAttributes;
using CatalogService.Application.Features.Categories.Queries;
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
    } // TODO: remove from tests

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
    public async Task<Result<CategoryDetailedResponse>> GetDetailedCategoryResponse(Guid id, CancellationToken ct = default)
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
            	c.path as Path,
            	cva.variant_attribute_id as VariantAttributeId,
                va.name as VariantAttributeName,
            	va.code as Code,
            	va.data_type_name as Datatype,
            	cva.display_order as DisplayOrder,
            	cva.is_required as IsRequired
            FROM public.categories c
            LEFT JOIN public.category_variant_attributes cva 
                ON c.id = cva.category_id
            LEFT JOIN public.variant_attribute_definitions va 
            	ON cva.variant_attribute_id = va.id 
            	AND va.is_deleted = false
            	AND va.is_active = true
            WHERE c.id = @id
                AND c.is_deleted = false
            	AND c.is_active = true

            ORDER BY c.level, c.name, cva.display_order
            """;

        var parameter = new { id };
        CategoryDetailedResponse? response = null;
        var command = new CommandDefinition(sql, parameters: parameter, cancellationToken: ct);
        await connection.QueryAsync<CategoryDetailedResponse, CategoryVariantForCategoryResponse, CategoryDetailedResponse>(command,
            (category, variant) => 
            {
                response ??= category with { Variants = [] };

                if (variant is not null)
                {
                    response.Variants?.Add(variant);
                }
                return category;
            }, 
            splitOn: "VariantAttributeId");

        if (response is null)
            return CategoryErrors.NotFound(id);

        return response;
    }

    public async Task<List<CategoryDetailedResponse>> GetCategoriesByIdAsync(
        List<Guid>? ids, CancellationToken ct = default)
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
            	c.path as Path,
            	cva.variant_attribute_id as VariantAttributeId,
                va.name as VariantAttributeName,
            	va.code as Code,
            	va.data_type_name as Datatype,
            	cva.display_order as DisplayOrder,
            	cva.is_required as IsRequired
            FROM public.categories c
            LEFT JOIN public.category_variant_attributes cva 
                ON c.id = cva.category_id
            LEFT JOIN public.variant_attribute_definitions va 
            	ON cva.variant_attribute_id = va.id 
            	AND va.is_deleted = false
            	AND va.is_active = true
            WHERE c.is_deleted = false
            	AND c.is_active = true
                AND (
                    @ids::uuid[] IS NULL
                    OR cardinality(@ids) = 0
                    OR c.id = ANY(@ids)
                )
            ORDER BY c.level, c.name, cva.display_order;
            """;

        var parameter = new { ids };
        var lookup = new Dictionary<Guid, CategoryDetailedResponse>();
        var command = new CommandDefinition(sql, parameters: parameter, cancellationToken: ct);

        await connection.QueryAsync<CategoryDetailedResponse, CategoryVariantForCategoryResponse, CategoryDetailedResponse>(command,
            (category, variant) => 
            {
                if (!lookup.TryGetValue(category.Id, out var existing))
                {
                    existing = category with { Variants = [] };
                    lookup.Add(existing.Id, existing);
                }

                if (variant is not null)
                {
                    existing.Variants!.Add(variant);
                }
                return existing;
            }, 
            splitOn: "VariantAttributeId");

        if (lookup.Count == 0)
            return [];

        return [.. lookup.Values];
    }
}
