using CatalogService.Domain.IRepositories;

namespace CatalogService.Domain.DomainService;

public sealed class CategoryDomainService(ICategoryRepository _repository) : ICategoryDomainService
{
    public async Task<Category> CreateCategoryAsync(
        string name,
        string slug,
        Guid? parentId = null,
        string? description = null,
        CancellationToken ct = default
        )
    {
        if (await _repository.ExistsAsync(e => e.Slug == slug, ct)) 
            throw new InvalidOperationException($"Category with slug '{slug}' already exists.");

        short level = 0;

        if (parentId.HasValue && parentId.Value != Guid.Empty)
        {
            var parentExists = await _repository.ExistsAsync(parentId.Value, ct);
            if (!parentExists)
                throw new InvalidOperationException("Parent category does not exist");

            var parents = await _repository.GetAllParentAsync(parentId.Value, ct);
            level = (short)(parents?.Count() ?? 0);
        }

        return Category.Create(
            name: name, 
            slug: slug, 
            level: level,
            parentId: parentId,
            description: description);
    }
}
