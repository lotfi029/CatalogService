
namespace CatalogService.Domain.IRepositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>?> GetAllParentAsync(Guid id, int maxDepth, CancellationToken ct = default);
}