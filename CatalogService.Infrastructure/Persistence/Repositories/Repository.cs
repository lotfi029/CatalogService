using CatalogService.Domain.Abstractions;
using CatalogService.Domain.IRepositories;
using System.Linq.Expressions;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public class Repository<T>(ApplicationDbContext _context) : IRepository<T>
    where T : Entity
{
    public Task<Guid> AddAsync(T entity, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
