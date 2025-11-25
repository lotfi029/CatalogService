
namespace CatalogService.Domain.IRepositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>?> GetAllParentAsync(Guid id, int maxDepth, CancellationToken ct = default);
    Task<IEnumerable<Category>> GetCategoryTree(Guid? id, CancellationToken ct = default);
}