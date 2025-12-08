using System.Linq.Expressions;

namespace CatalogService.Infrastructure.Persistence.Repositories;

internal class ProductCategoryRepository(ApplicationDbContext context) : IProductCategoryRepository
{
    public void Add(ProductCategories productCategory)
    {
        ArgumentNullException.ThrowIfNull(productCategory);
        context.Add(productCategory);
    }

    public void AddRange(IEnumerable<ProductCategories> productCategories)
    {
        ArgumentNullException.ThrowIfNull(productCategories);
        context.AddRange(productCategories);
    }
    public void RemoveRange(IEnumerable<ProductCategories> productCategories)
    {
        ArgumentNullException.ThrowIfNull(productCategories);
        context.RemoveRange(productCategories);
    }
    public void Remove(ProductCategories productCategory)
    {
        ArgumentNullException.ThrowIfNull(productCategory);
        context.Remove(productCategory);
    }

    public void Update(ProductCategories productCategory)
    {
        ArgumentNullException.ThrowIfNull(productCategory);
        context.Update(productCategory);
    }
    public async Task<bool> ExistsAsync(Guid productId, Guid categoryId, CancellationToken ct = default)
    {
        return await context.ProductCategories
            .AnyAsync(pc => pc.ProductId == productId & pc.CategoryId == categoryId, ct);
    }
    public async Task<bool> ExistsAsync(Expression<Func<ProductCategories, bool>> predicate, CancellationToken ct = default)
    {
        return await context.ProductCategories
            .AnyAsync(predicate: predicate, cancellationToken: ct);
    }

    public async Task<ProductCategories?> GetAsync(Guid productId, Guid categoryId, CancellationToken ct = default)
    {
        return await context.ProductCategories
            .FindAsync([productId, categoryId], cancellationToken: ct);
    }

    public async Task<IEnumerable<ProductCategories>> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct = default)
    {
        return await context.ProductCategories
            .Where(pc => pc.CategoryId == categoryId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ProductCategories>> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        return await context.ProductCategories
            .Where(pc => pc.ProductId == productId)
            .ToListAsync(ct);
    }


    public async Task<int> RemoveAllByCategoryAsync(Guid categoryId, CancellationToken ct = default)
    {
        return await context.ProductCategories
            .Where(pc => pc.CategoryId == categoryId)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<int> RemoveAllByProductAsync(Guid productId, CancellationToken ct = default)
    {
        return await context.ProductCategories
            .Where(pc => pc.ProductId == productId)
            .ExecuteDeleteAsync(ct); 
    }
    public async Task<int> UpdatePrimaryAsync(Guid productId, Guid categoryId, bool isPrimary, CancellationToken ct = default)
    {
        return await context.ProductCategories
            .Where(pc => pc.CategoryId == categoryId && pc.ProductId == productId)
            .ExecuteUpdateAsync(e =>
        {
            e.SetProperty(pc => pc.IsPrimary, isPrimary);
        }, ct);
    }
}
