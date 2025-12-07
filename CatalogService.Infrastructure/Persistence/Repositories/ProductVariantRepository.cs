namespace CatalogService.Infrastructure.Persistence.Repositories;

internal sealed class ProductVariantRepository(ApplicationDbContext context) : IProductVariantRepository
{
    private readonly DbSet<ProductVariant> _dbSet = context.Set<ProductVariant>();

    public void Add(ProductVariant productVariant)
    {
        ArgumentNullException.ThrowIfNull(productVariant);
        _dbSet.Add(productVariant);
    }

    public void AddRange(ProductVariant[] variants)
    {
        ArgumentNullException.ThrowIfNull(variants);
        _dbSet.AddRange(variants);
    }

    public void Delete(ProductVariant productVariant)
    {
        ArgumentNullException.ThrowIfNull(productVariant);
        _dbSet.Remove(productVariant);
    }

    public void DeleteRange(ProductVariant[] variants)
    {
        ArgumentNullException.ThrowIfNull(variants);
        _dbSet.RemoveRange(variants);
    }

    public async Task<IEnumerable<ProductVariant>> GetAllAsync(Guid productId, CancellationToken ct = default)
    {
        return await _dbSet.Where(e => e.ProductId == productId).ToListAsync(ct);
    }

    public async Task<ProductVariant?> GetById(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken: ct);
    }

    public void Update(ProductVariant productVariant)
    {
        ArgumentNullException.ThrowIfNull(productVariant);
        _dbSet.Update(productVariant);
    }

    public void UpdateRange(ProductVariant[] variants)
    {
        ArgumentNullException.ThrowIfNull(variants);
        _dbSet.UpdateRange(variants);
    }
}