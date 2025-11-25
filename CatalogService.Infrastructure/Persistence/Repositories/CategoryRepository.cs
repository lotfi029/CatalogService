using CatalogService.Domain.IRepositories;
using System.Collections.Immutable;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository(ApplicationDbContext context)
    : Repository<Category>(context), ICategoryRepository
{
    public async Task<IEnumerable<Category>?> GetAllParentAsync(Guid id, int maxDepth, CancellationToken ct = default)
    {
        FormattableString sql = $"""
            WITH RECURSIVE parent_chain AS (
                SELECT c.*, 0 as depth
                from categories c
                WHERE c.id = {id}
                UNION ALL
                SELECT c.*, pc.depth + 1
                FROM categories c
                INNER JOIN parent_chain pc ON c.id = pc.parent_id
                WHERE pc.depth < {maxDepth}
            )
            SELECT * FROM parent_chain
            ORDER BY depth
            """;

        var parents = await _context.Categories.FromSqlInterpolated(sql).ToListAsync(ct);

        return parents;
    }
    public async Task<IEnumerable<Category>> GetCategoryTree (Guid? id, CancellationToken ct = default)
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
                INNER JOIN category_tree ct ON c.parent_id = ct.id
                WHERE c.is_deleted = false
            )
            SELECT * FROM category_tree 
            """;
        
        var categories = await _context.Categories.FromSqlInterpolated(sql).ToListAsync(ct);

        //List<CategoryDTO> buildTree(List<Category> categories, Guid? parentId)
        //{
        //    return [.. categories.Where(e => e.ParentId == parentId)
        //    .Select(c => new CategoryDTO
        //    {
        //        Id = c.Id,
        //        Name = c.Name,
        //        Description = c.Description,
        //        Slug = c.Slug,
        //        Path = c.Path,
        //        Level = c.Level,
        //        CreatedBy = c.CreatedBy,
        //        LastUpdatedBy = c.LastUpdatedBy,
        //        CreatedAt = c.CreatedAt,
        //        LastUpdatedAt = c.LastUpdatedAt,
        //        DeletedAt = c.DeletedAt,
        //        IsDeleted = c.IsDeleted,
        //        Childrens = buildTree(categories, parentId) ?? null
        //    })];
        //}
        var parentId = categories.Where(e => e.Id == id).FirstOrDefault();

        var children = categories.Where(e => e.Id != id)
            .GroupBy(e => e.ParentId)
            .Select(x => new CategoryDTO
            {
                Childrens = [.. x.Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    ParentId = c.ParentId,
                    Name = c.Name
                })]
            });


        foreach (var child in categories)
        {
            Console.WriteLine();
        }


        var distinct = categories.Select(x => x.ParentId).Distinct().ToList();


        return categories;
    }
    class CategoryDTO
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Path { get;   set; } = string.Empty;
        public short Level { get; set; } = 0;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<CategoryDTO>? Childrens { get; set; }

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