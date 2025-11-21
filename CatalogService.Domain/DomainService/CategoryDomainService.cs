using CatalogService.Domain.Errors;
using CatalogService.Domain.IRepositories;

namespace CatalogService.Domain.DomainService;

public sealed class CategoryDomainService(ICategoryRepository _repository) : ICategoryDomainService
{
    public async Task<Result<Category>> CreateCategoryAsync(
        string name,
        string slug,
        Guid? parentId = null,
        string? description = null,
        CancellationToken ct = default
        )
    {
        if (await _repository.ExistsAsync(e => e.Slug == slug, ct))
            return CategoryErrors.SlugAlreadyExist(slug);

        short level = 0;

        if (parentId.HasValue && parentId.Value != Guid.Empty)
        {
            var parentExists = await _repository.ExistsAsync(parentId.Value, ct);
            if (!parentExists)
                return CategoryErrors.ParentNotFound(parentId.Value);

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
