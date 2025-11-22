namespace CatalogService.Domain.DomainService;

public interface ICategoryDomainService
{
    Task<Result<Category>> CreateCategoryAsync(string name, string slug, bool isActive, Guid? parentId = null, int? maxDepth = null, string? description = null, CancellationToken ct = default);
}
