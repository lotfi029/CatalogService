using CatalogService.Domain.Abstractions;
using System.Linq.Expressions;

namespace CatalogService.Domain.IRepositories;
public interface IRepository<T> where T : Entity
{
    Task<Guid> AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    //Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    //Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    //Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    //Task<T?> FindAsync(CancellationToken ct = default, params object?[] keys);
}





