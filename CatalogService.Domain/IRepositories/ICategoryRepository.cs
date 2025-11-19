
namespace CatalogService.Domain.IRepositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>?> GetAllParentAsync(Guid id, CancellationToken ct = default);
}





