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
        int maxDepth = 100,
        string? description = null,
        CancellationToken ct = default
        )
    {
        if (await repository.ExistsAsync(e => e.Slug == slug, ct))
            return CategoryErrors.SlugAlreadyExist(slug);

        short level = 0;

        if (parentId.HasValue && parentId.Value != Guid.Empty)
        {
            var parents = await repository.GetAllParentAsync(parentId.Value, maxDepth, ct);
            
            if (parents is null || !parents.Any())
                return CategoryErrors.ParentNotFound(parentId.Value);

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

    public async Task<Result<Category>> MoveToNewParent(Category childCat, Guid newParentId, CancellationToken ct = default)
    {
        // logic
        var allParents = await repository.GetAllParentAsync(newParentId, int.MaxValue, ct);
        
        if (allParents is null || !allParents.Any())
            return CategoryErrors.ParentNotFound(newParentId);

        throw new NotImplementedException();
    }
}
