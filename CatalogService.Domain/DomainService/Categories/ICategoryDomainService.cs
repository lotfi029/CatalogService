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
}
