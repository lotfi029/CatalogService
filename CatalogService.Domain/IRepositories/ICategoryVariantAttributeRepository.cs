namespace CatalogService.Domain.IRepositories;

public interface ICategoryVariantAttributeRepository
{
    void Add(CategoryVariantAttribute categoryVariant);
    void AddRange(IEnumerable<CategoryVariantAttribute> categoryVariants);
    void Update(CategoryVariantAttribute categoryVariant);
    void Remove(CategoryVariantAttribute categoryVariant);
    void RemoveRange(IEnumerable<CategoryVariantAttribute> categoryVariants);
    Task<bool> ExistsAsync(Guid categoryId, Guid variantAttributeId, CancellationToken ct = default);
    Task<IEnumerable<CategoryVariantAttribute>> GetByCategoryIdAsync(
        Guid categoryId,
        CancellationToken ct = default);
    Task<IEnumerable<CategoryVariantAttribute>> GetByVariantAttributeIdAsync(
        Guid variantAttributeId,
        CancellationToken ct = default);
    Task<CategoryVariantAttribute?> GetAsync(
        Guid categoryId,
        Guid variantAttributeId,
        CancellationToken ct = default);
    Task<int> RemoveAllByCategoryAsync(Guid categoryId, CancellationToken ct = default);
    Task<int> RemoveAllByVariantAttributeAsync(Guid variantAttributeId, CancellationToken ct = default);
}