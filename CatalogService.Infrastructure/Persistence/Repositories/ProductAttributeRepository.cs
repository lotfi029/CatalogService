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
        Guid productAttributesId,
        CancellationToken ct = default)
    {
        return await _dbSet
            .AnyAsync(
                e => e.ProductId == productId && e.AttributeId == productAttributesId,
                cancellationToken: ct);
    }

    public async Task<IEnumerable<ProductAttributes>> GetAllAsync(
        Guid productId,
        CancellationToken ct = default)
    {
        return await _dbSet
            .Where(e => e.ProductId == productId)
            .ToListAsync(ct);
    }

    public async Task<ProductAttributes?> GetById(
        Guid productId,
        CancellationToken ct = default)
    {
        return await _dbSet.FindAsync([productId], cancellationToken: ct);
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
}
