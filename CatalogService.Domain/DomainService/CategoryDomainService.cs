using CatalogService.Domain.Errors;
using CatalogService.Domain.IRepositories;

namespace CatalogService.Domain.DomainService;

public sealed class CategoryDomainService(ICategoryRepository repository) : ICategoryDomainService
{
    public async Task<Result<Category>> CreateCategoryAsync(
        string name,
        string slug,
        bool isActive,
        Guid? parentId = null,
        int? maxDepth = null,
        string? description = null,
        CancellationToken ct = default
        )
    {
        if (await repository.ExistsAsync(e => e.Slug == slug, ct))
            return CategoryErrors.SlugAlreadyExist(slug);

        short level = 0;

        if (parentId.HasValue && parentId.Value != Guid.Empty)
        {
            var parentExists = await repository.ExistsAsync(parentId.Value, ct);
            if (!parentExists)
                return CategoryErrors.ParentNotFound(parentId.Value);

            var parents = await repository.GetAllParentAsync(parentId.Value, maxDepth ?? 100, ct);

            level = (short)(parents?.Count() ?? 0);
        }

        return Category.Create(
            name: name, 
            slug: slug, 
            isActive: isActive,
            level: level,
            parentId: parentId,
            description: description);
    }
}
