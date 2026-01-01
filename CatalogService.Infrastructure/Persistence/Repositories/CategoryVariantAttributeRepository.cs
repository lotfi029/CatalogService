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
    public async Task<bool> ExistsAsync(
        Expression<Func<CategoryVariantAttribute, bool>> predicate, 
        CancellationToken ct = default)
    {
        return await _dbSet
            .AnyAsync(predicate, ct);
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
    public async Task<IEnumerable<CategoryVariantAttribute>> GetAvaliableVariantAsync(HashSet<Guid> categoryIds, CancellationToken ct = default)
    {
        return await _dbSet
            .Where(cv => categoryIds.Contains(cv.CategoryId))
            .Include(cv => cv.VariantAttribute)
            .AsNoTracking()
            .ToListAsync(ct);
    }
    public async Task<IEnumerable<CategoryVariantAttribute>> GetAsync(
        HashSet<Guid> categoryIds,
        HashSet<Guid> variants,
        CancellationToken ct = default)
    {
        return await _dbSet
            .Where(cv => categoryIds.Contains(cv.CategoryId) && variants.Contains(cv.VariantAttributeId))
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
    public async Task<int> ExecuteDeleteAsync(Expression<Func<CategoryVariantAttribute, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.Where(predicate).ExecuteDeleteAsync(ct);
}
