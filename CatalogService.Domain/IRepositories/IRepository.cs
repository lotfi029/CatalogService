using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CatalogService.Domain.IRepositories;
public interface IRepository<T> where T : Entity
{
    Guid Add(T entity);
    void AddRange(IEnumerable<T> entityList);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setProperityCalls, CancellationToken ct = default);
    Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T?> FindByIdAsync(Guid id, CancellationToken ct = default);
    Task<T?> FindAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
}