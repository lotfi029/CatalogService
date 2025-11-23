namespace CatalogService.Domain.DomainService;

public interface ICategoryDomainService
{
    Task<Result<Category>> CreateCategoryAsync(string name, string slug, bool isActive, Guid? parentId = null, int maxDepth = 100, string? description = null, CancellationToken ct = default);
}
