using CatalogService.Domain.IRepositories;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T>
    where T : Entity
{
    protected ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(
        ApplicationDbContext context
        )
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    #region operation
    public Guid Add(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        _dbSet.Add(entity);

        return entity.Id;
    }
    public void AddRange(IEnumerable<T> entityList)
    {
        ArgumentNullException.ThrowIfNull(entityList);
        _dbSet.AddRange(entityList);
    }
    public void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Update(entity);
    }
    public void UpdateRange(IEnumerable<T> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.UpdateRange(entity);
    }
    public void Remove(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
    }
    public void RemoveRange(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        _dbSet.RemoveRange(entities);
    }

    public async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return await _dbSet.Where(predicate)
            .ExecuteDeleteAsync(ct);
    }

    //public async Task<int> ExecuteUpdateAsync(
    //    Expression<Func<T, bool>> predicate, 
    //    Action<UpdateSettersBuilder<T>> setPropertyCalls, 
    //    CancellationToken ct = default) 
    //{
    //    ArgumentNullException.ThrowIfNull(predicate);

    //    return await _dbSet.Where(predicate)
    //        .ExecuteUpdateAsync(setPropertyCalls, ct);

    //}
    #endregion
    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.AnyAsync(e => e.Id == id, ct);
    }
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return await _dbSet.AnyAsync(predicate, ct);
    }

    public async Task<T?> FindByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync([id], ct);
    }
    public async Task<T?> FindAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(expression, ct);
    }
    protected IQueryable<T> Query() => _dbSet.AsQueryable();
}
