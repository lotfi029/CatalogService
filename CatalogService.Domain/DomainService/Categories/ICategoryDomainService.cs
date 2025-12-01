namespace CatalogService.Domain.DomainService.Categories;

public interface ICategoryDomainService
{
    Task<Result<Category>> AddVariantAttributeToCategoryAsync(Category category, Guid variantId, short displayOrder, bool isRequired, CancellationToken ct = default);
    Task<Result<Category>> CreateCategoryAsync(
        string name, 
        string slug, 
        bool isActive, 
        Guid? parentId = null,
        string? description = null, 
        CancellationToken ct = default);
    Task<Result<List<Category>>> MoveToNewParent(Guid id, Category parent, CancellationToken ct = default);
    Task<Result> RemoveCategoryVariantAsync(Guid id, Guid variantId, CancellationToken ct = default);
    Task<Result> UpdateCategoryVariantAsync(Guid id, Guid variantId, short? displayOrder, bool? isRequired, CancellationToken ct = default);
}
