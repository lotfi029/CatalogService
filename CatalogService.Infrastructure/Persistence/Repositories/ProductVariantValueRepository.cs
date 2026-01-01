using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CatalogService.Infrastructure.Persistence.Repositories;

internal sealed class ProductVariantValueRepository(ApplicationDbContext context) : IProductVariantValueRepository
{
    private readonly DbSet<ProductVariantValue> _dbSet = context.Set<ProductVariantValue>();

    public void Add(ProductVariantValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _dbSet.Add(value);
    }

    public void AddRange(ProductVariantValue[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _dbSet.AddRange(values);
    }

    public void Delete(ProductVariantValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _dbSet.Remove(value);
    }

    public void DeleteRange(ProductVariantValue[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _dbSet.RemoveRange(values);
    }
    public void Update(ProductVariantValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _dbSet.Update(value);
    }

    public void UpdateRange(ProductVariantValue[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _dbSet.UpdateRange(values);
    }
    public async Task<int> ExecuteUpdateAsync(
        Expression<Func<ProductVariantValue, bool>> predicate,
        Action<UpdateSettersBuilder> action,
        CancellationToken ct = default)
    {
        return await _dbSet
            .Where(predicate)
            .ExecuteUpdateAsync(action, ct);
    }
    public async Task<int> ExecuteDeleteAsync(Expression<Func<ProductVariantValue, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbSet
            .Where(predicate)
            .ExecuteDeleteAsync(ct);
    }
    public async Task<bool> ExistsAsync(Guid id, Guid valueId, CancellationToken ct = default)
    {
        return await _dbSet
            .AnyAsync(e => e.Id == id && e.Id == valueId, cancellationToken: ct);
    }
    public async Task<bool> ExistsAsync(Expression<Func<ProductVariantValue, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbSet
            .AnyAsync(predicate, cancellationToken: ct);
    }

    public async Task<IEnumerable<ProductVariantValue>> GetAllAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.Where(e => e.Id == id).ToListAsync(ct);
    }

    public async Task<ProductVariantValue?> GetById(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken: ct);
    }
    
}