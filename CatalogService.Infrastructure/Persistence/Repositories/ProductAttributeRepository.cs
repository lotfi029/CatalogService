using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CatalogService.Infrastructure.Persistence.Repositories;

internal sealed class ProductAttributeRepository(ApplicationDbContext context) : IProductAttributeRepository
{
    private readonly DbSet<ProductAttributes> _dbSet = context.Set<ProductAttributes>();
    public void Add(ProductAttributes productAttributes)
    {
        ArgumentNullException.ThrowIfNull(productAttributes);
        _dbSet.Add(productAttributes);
    }

    public void AddRange(ProductAttributes[] products)
    {
        ArgumentNullException.ThrowIfNull(products);
        _dbSet.AddRange(products);
    }
    public void Update(ProductAttributes productAttributes)
    {
        ArgumentNullException.ThrowIfNull(productAttributes);
        _dbSet.Update(productAttributes);
    }

    public void UpdateRange(ProductAttributes[] products)
    {
        ArgumentNullException.ThrowIfNull(products);
        _dbSet.UpdateRange(products);
    }
    public void Delete(ProductAttributes productAttributes)
    {
        ArgumentNullException.ThrowIfNull(productAttributes);
        _dbSet.Remove(productAttributes);
    }

    public void DeleteRange(ProductAttributes[] products)
    {
        ArgumentNullException.ThrowIfNull(products);
        _dbSet.RemoveRange(products);
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<ProductAttributes, bool>> predicate,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return await _dbSet.AnyAsync(predicate, cancellationToken: ct);
    }

    public async Task<bool> ExistsAsync(
        Guid productId,
        Guid attributeId,
        CancellationToken ct = default)
    {
        return await _dbSet
            .AnyAsync(
                e => e.ProductId == productId && e.AttributeId == attributeId,
                cancellationToken: ct);
    }

    public async Task<IEnumerable<ProductAttributes>> GetAllByProductIdAsync(
        Guid productId,
        CancellationToken ct = default)
    {
        return await _dbSet
            .Where(e => e.ProductId == productId)
            .ToListAsync(ct);
    }

    public async Task<ProductAttributes?> GetById(
        Guid productId,
        Guid attributeId,
        CancellationToken ct = default)
    {
        return await _dbSet.FindAsync([productId, attributeId], cancellationToken: ct);
    }
    public async Task<int> ExecuteDeleteAsync(Expression<Func<ProductAttributes, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.Where(predicate)
            .ExecuteDeleteAsync(cancellationToken: ct);


    public async Task<int> ExecuteUpdateAsync(
        Expression<Func<ProductAttributes, bool>> predicate,
        Action<UpdateSettersBuilder<ProductAttributes>> action,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(action);
        return await _dbSet.Where(predicate)
                .ExecuteUpdateAsync(action, cancellationToken: ct);
    }
}
