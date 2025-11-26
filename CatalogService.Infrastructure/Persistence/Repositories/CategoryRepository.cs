using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository(ApplicationDbContext context)
    : Repository<Category>(context), ICategoryRepository
{
    public async Task<IEnumerable<Category>?> GetAllParentAsync(Guid id, CancellationToken ct = default)
    {
        FormattableString sql = $"""
            WITH RECURSIVE parent_chain AS (
                SELECT c.*
                from categories c
                WHERE c.id = {id}
                    AND is_deleted = false
                UNION ALL
                SELECT c.*
                FROM categories c
                INNER JOIN parent_chain pc 
                    ON c.id = pc.parent_id
                WHERE c.is_deleted = false 
            )
            SELECT * FROM parent_chain
            order by level
            """;

        var parents = await _context.Categories.FromSqlInterpolated(sql).ToListAsync(ct);

        return parents;
    }
    public async Task<List<Category>> GetChildrenAsync (Guid? id, CancellationToken ct = default)
    {
        FormattableString sql = $"""
            WITH RECURSIVE category_tree AS (
                SELECT  *
                FROM public.categories
                WHERE id = {id}
                  AND is_deleted = false
                UNION ALL
                SELECT c.*
                FROM public.categories c
                INNER JOIN category_tree ct 
                    ON c.parent_id = ct.id
                WHERE c.is_deleted = false 
            )
            SELECT * FROM category_tree t
            ORDER BY t.level
            """;
        
        var categories = await _context.Categories
            .FromSqlInterpolated(sql)
            .ToListAsync(ct);

        return categories;
    }
}

class CatagorySqlFunction
{
    public static string GetAllChilderFN =>
        """
        CREATE OR REPLACE FUNCTION build_category_tree(parent_uuid UUID)
        RETURNS jsonb AS $$
        DECLARE
            result jsonb;
        BEGIN
            SELECT jsonb_agg(
                jsonb_build_object(
                    'id', c.id,
        			'parent_id', c.parent_id,
        			'name', c.name,
        			'description', c.description,
        			'slug', c.slug,
        			'path', c.path,
        			'level', c.level,
        			'created_by', c.created_by,
        			'created_at', c.created_at,
        			'last_updated_by', c.last_updated_by,
        			'last_updated_at', c.last_updated_at,
        			'is_active', c.is_active,
        			'is_deleted', c.is_deleted,
        			'deleted_by', c.deleted_by,
        			'deleted_at', c.deleted_at,
                    'children', build_category_tree(c.id)
                )
            )
            INTO result
            FROM categories c
            WHERE c.parent_id = parent_uuid OR (parent_uuid IS NULL AND c.parent_id IS NULL);

            RETURN result;
        END;
        $$ LANGUAGE plpgsql;
        """;
}