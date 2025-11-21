using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository(
    ApplicationDbContext context,
    ILogger<Repository<Category>> repositoryLogger)
    : Repository<Category>(context, repositoryLogger), ICategoryRepository
{
    public async Task<IEnumerable<Category>?> GetAllParentAsync(Guid id, CancellationToken ct = default)
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
                WHERE pc.depth < 100
            )
            SELECT * FROM parent_chain
            ORDER BY depth
            """;

        var parents = await _context.Categories.FromSqlInterpolated(sql).ToListAsync(ct);

        return parents;
    }
}