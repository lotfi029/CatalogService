using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class CategoryVariantAttributeRepository(ApplicationDbContext context) : ICategoryVariantAttributeRepository
{
    public void Add(CategoryVariantAttribute categoryVariant)
    {
        context.Add(categoryVariant);
    }

    public void AddRange(IEnumerable<CategoryVariantAttribute> categoryVariants)
    {
        context.AddRange(categoryVariants);
    }

    public async Task<bool> ExistsAsync(Guid categoryId, Guid variantAttributeId, CancellationToken ct = default)
    {
        return await context.CategoryVariantAttributes
            .AnyAsync(cv => cv.CategoryId == categoryId && cv.VariantAttributeId == variantAttributeId, ct);
    }

    public async Task<CategoryVariantAttribute?> GetAsync(Guid categoryId, Guid variantAttributeId, CancellationToken ct = default)
    {
        return await context.CategoryVariantAttributes.FindAsync([categoryId, variantAttributeId], ct);
    }

    public async Task<IEnumerable<CategoryVariantAttribute>> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct = default)
    {
        return await context.CategoryVariantAttributes
            .Where(cv => cv.CategoryId == categoryId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<CategoryVariantAttribute>> GetByVariantAttributeIdAsync(Guid variantAttributeId, CancellationToken ct = default)
    {
        return await context.CategoryVariantAttributes
            .Where(cv => cv.VariantAttributeId == variantAttributeId)
            .ToListAsync(ct);
    }

    public void Remove(CategoryVariantAttribute categoryVariant)
    {
        context.Remove(categoryVariant);
    }

    public async Task<int> RemoveAllByCategoryAsync(Guid categoryId, CancellationToken ct = default)
    {
        return await context.CategoryVariantAttributes
            .Where(e => e.CategoryId == categoryId)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<int> RemoveAllByVariantAttributeAsync(Guid variantAttributeId, CancellationToken ct = default)
    {
        return await context.CategoryVariantAttributes
            .Where(e => e.VariantAttributeId == variantAttributeId)
            .ExecuteDeleteAsync(ct);
    }

    public void RemoveRange(IEnumerable<CategoryVariantAttribute> categoryVariants)
    {
        context.RemoveRange(categoryVariants);
    }

    public void Update(CategoryVariantAttribute categoryVariant)
    {
        context.Update(categoryVariant);
    }
}
