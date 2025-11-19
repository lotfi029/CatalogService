namespace CatalogService.Domain.DomainService;

public interface ICategoryDomainService
{
    Task<Category> CreateCategoryAsync(string name, string slug, Guid? parentId = null, string? description = null, CancellationToken ct = default);
}
