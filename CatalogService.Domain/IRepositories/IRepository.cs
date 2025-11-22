using CatalogService.Domain.Abstractions;
using System.Linq.Expressions;

namespace CatalogService.Domain.IRepositories;
public interface IRepository<T> where T : Entity
{
    Task<Guid> AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(List<T> entities, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
    Task ExcuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T?> FindByIdAsync(Guid id, CancellationToken ct = default);
    Task<T?> FindAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
}