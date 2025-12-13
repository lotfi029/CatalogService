using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class CategoryVariantAttributeRepository(ApplicationDbContext context) : ICategoryVariantAttributeRepository
{
    private readonly DbSet<CategoryVariantAttribute> _dbSet = context.Set<CategoryVariantAttribute>();
    public void Add(CategoryVariantAttribute categoryVariant)
    {
        ArgumentNullException.ThrowIfNull(categoryVariant);
        _dbSet.Add(categoryVariant);
    }

    public void AddRange(IEnumerable<CategoryVariantAttribute> categoryVariants)
    {
        ArgumentNullException.ThrowIfNull(categoryVariants);
        _dbSet.AddRange(categoryVariants);
    }

    public void Update(CategoryVariantAttribute categoryVariant)
    {
        ArgumentNullException.ThrowIfNull(categoryVariant);
        _dbSet.Update(categoryVariant);
    }

    public void Remove(CategoryVariantAttribute categoryVariant)
    {
        ArgumentNullException.ThrowIfNull(categoryVariant);
        _dbSet.Remove(categoryVariant);
    }

    public void RemoveRange(IEnumerable<CategoryVariantAttribute> categoryVariants)
    {
        ArgumentNullException.ThrowIfNull(categoryVariants);
        _dbSet.RemoveRange(categoryVariants);
    }
    public async Task<int> ExecuteUpdateAsync(
        Expression<Func<CategoryVariantAttribute, bool>> predicate,
        Action<UpdateSettersBuilder<CategoryVariantAttribute>> action,
        CancellationToken ct = default)
    {
        return await _dbSet
            .Where(predicate)
            .ExecuteUpdateAsync(action, ct);
    }
    public async Task<bool> ExistsAsync(
        Guid categoryId,
        Guid variantAttributeId,
        CancellationToken ct = default)
    {
        return await _dbSet.AnyAsync(
            cv => cv.CategoryId == categoryId && cv.VariantAttributeId == variantAttributeId,
            ct);
    }

    public async Task<IEnumerable<CategoryVariantAttribute>> GetByCategoryIdAsync(
        Guid categoryId,
        CancellationToken ct = default)
    {
        return await _dbSet
            .Where(cv => cv.CategoryId == categoryId)
            .OrderBy(cv => cv.DisplayOrder)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<CategoryVariantAttribute>> GetByVariantAttributeIdAsync(
        Guid variantAttributeId,
        CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(cv => cv.VariantAttributeId == variantAttributeId)
            .ToListAsync(ct);
    }
    public async Task<IEnumerable<CategoryVariantAttribute>> GetCategoryVariantsByCategoryIdId(Guid categoryId, CancellationToken ct = default)
    {
        return await _dbSet.Where(cv => cv.CategoryId == categoryId)
            .Include(cv => cv.VariantAttribute)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<CategoryVariantAttribute?> GetAsync(
        Guid categoryId,
        Guid variantAttributeId,
        CancellationToken ct = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(
                cv => cv.CategoryId == categoryId && cv.VariantAttributeId == variantAttributeId,
                ct);
    }

    public async Task<int> RemoveAllByCategoryAsync(Guid categoryId, CancellationToken ct = default)
    {
        return await _dbSet
            .Where(cv => cv.CategoryId == categoryId)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<int> RemoveAllByVariantAttributeAsync(
        Guid variantAttributeId,
        CancellationToken ct = default)
    {
        return await _dbSet
            .Where(cv => cv.VariantAttributeId == variantAttributeId)
            .ExecuteDeleteAsync(ct);
    }
}
