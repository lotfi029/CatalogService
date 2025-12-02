
namespace CatalogService.Domain.DomainService.Categories;

public interface ICategoryDomainService
{
    Task<Result<Category>> CreateCategoryAsync(
        string name, 
        string slug, 
        bool isActive, 
        Guid? parentId = null,
        string? description = null, 
        CancellationToken ct = default);
    Task<Result<List<Category>>> MoveToNewParent(Guid id, Category parent, CancellationToken ct = default);

    Task<Result> AddVariantAttributeToCategoryAsync(Guid id, Guid variantId, short displayOrder, bool isRequired, CancellationToken ct = default);
    Task<Result> RemoveVariantAttributeFromCategoryAsync(Guid id, Guid variantId, CancellationToken ct = default);
    Task<Result> UpdateCategoryVariantAttributeAsync(Guid id, Guid variantId, short displayOrder, bool isRequired, CancellationToken ct = default);
    Task<Result> AddBulkCategoryVariantAttributeAsync(Guid id, IEnumerable<(Guid variantId, bool isRequired, short displayOrder)> variants, CancellationToken ct = default);
}
