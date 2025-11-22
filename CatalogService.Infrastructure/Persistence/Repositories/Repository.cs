using CatalogService.Domain.Abstractions;
using CatalogService.Domain.IRepositories;
using System.Linq.Expressions;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T>
    where T : Entity
{
    protected ApplicationDbContext _context;
    private readonly ILogger<Repository<T>> _logger;
    private readonly DbSet<T> _dbSet;

    public Repository(
        ApplicationDbContext context,
        ILogger<Repository<T>> logger
        )
    {
        _context = context;
        _logger = logger;
        _dbSet = _context.Set<T>();
    }
    public async Task<Guid> AddAsync(T entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dbSet.AddAsync(entity, ct);
        return entity.Id;
    }
    public async Task AddRangeAsync(List<T> entityList, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entityList);
        await _dbSet.AddRangeAsync(entityList, ct);
    }

    public async Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
    }
    public async Task ExcuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        await _dbSet.Where(predicate)
            .ExecuteDeleteAsync(ct);
    }

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
            .SingleOrDefaultAsync(expression, ct);
    }
    public Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Update(entity);

        return Task.CompletedTask;
    }
    protected IQueryable<T> Query() => _dbSet.AsQueryable();
}
