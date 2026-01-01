using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CatalogService.Domain.IRepositories;

public interface ICategoryVariantAttributeRepository
{
    void Add(CategoryVariantAttribute categoryVariant);
    void AddRange(IEnumerable<CategoryVariantAttribute> categoryVariants);
    void Update(CategoryVariantAttribute categoryVariant);
    void Remove(CategoryVariantAttribute categoryVariant);
    void RemoveRange(IEnumerable<CategoryVariantAttribute> categoryVariants);
    Task<bool> ExistsAsync(Guid categoryId, Guid variantAttributeId, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<CategoryVariantAttribute, bool>> predicate, CancellationToken ct = default);
    
    Task<IEnumerable<CategoryVariantAttribute>> GetAvaliableVariantAsync(
        HashSet<Guid> categoryId,
        CancellationToken ct = default);
    Task<IEnumerable<CategoryVariantAttribute>> GetAsync(
        HashSet<Guid> categoryIds,
        HashSet<Guid> variants,
        CancellationToken ct = default);
    Task<IEnumerable<CategoryVariantAttribute>> GetByVariantAttributeIdAsync(
        Guid variantAttributeId,
        CancellationToken ct = default);
    Task<CategoryVariantAttribute?> GetAsync(
        Guid categoryId,
        Guid variantAttributeId,
        CancellationToken ct = default);
    Task<int> ExecuteDeleteAsync(Expression<Func<CategoryVariantAttribute, bool>> predicate, CancellationToken ct = default);
    Task<int> ExecuteUpdateAsync(Expression<Func<CategoryVariantAttribute, bool>> predicate, Action<UpdateSettersBuilder<CategoryVariantAttribute>> action, CancellationToken ct = default);
}
