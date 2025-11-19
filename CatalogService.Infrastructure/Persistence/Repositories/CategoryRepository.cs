using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository(ApplicationDbContext context)
    : Repository<Category>(context), ICategoryRepository
{
    public async Task<IEnumerable<Category>?> GetAllParentAsync(Guid id, CancellationToken ct = default)
    {
        var parents = new List<Category>();
        var category = await GetByIdAsync(id, ct);
        while (category?.ParentId is not null)
        {
            category = await GetByIdAsync(category.ParentId.Value, ct);
            if (category is not null)
            {
                parents.Add(category);
            }
        }
        return parents;
    }
}