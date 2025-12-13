using CatalogService.Domain.Contants;

namespace CatalogService.Domain.DomainService.Categories;

public sealed class CategoryDomainService(
    ICategoryRepository repository,
    IVariantAttributeRepository variantAttributeRepository,
    ICategoryVariantAttributeRepository categoryVariantRepository,
    IProductCategoryRepository productCategoryRepository) : ICategoryDomainService
{
    #region category
    public async Task<Result<Category>> CreateCategoryAsync(
        string name,
        string slug,
        bool isActive,
        Guid? parentId = null,
        string? description = null,
        CancellationToken ct = default)
    {
        if (await repository.ExistsAsync(e => e.Slug == slug, [QueryFilterConsts.SoftDeleteFilter], ct))
            return CategoryErrors.SlugAlreadyExist(slug);

        short level = 0;
        var path = "";
        if (parentId.HasValue && parentId.Value != Guid.Empty)
        {
            if (await repository.FindAsync(parentId.Value, null, ct) is not { } parent)
                return CategoryErrors.ParentNotFound(parentId.Value);
            
            path = parent.Path!;
            
            level = (short)(parent.Level + 1);
        }

        return Category.Create(
            name: name,
            slug: slug,
            isActive: isActive,
            level: level,
            parentId: parentId,
            parentPath: path,
            description: description);
    }

    public async Task<Result<List<Category>>> MoveToNewParent(
        Guid id, 
        Guid parentId,
        CancellationToken ct = default)
    {
        if (await repository.FindAsync(parentId, null, ct) is not { } parent)
            return CategoryErrors.NotFound(parentId);

        var categoryTree = await repository.GetChildrenAsync(id, ct);

        if (categoryTree is null || categoryTree.Count == 0)
            return CategoryErrors.NotFound(id);

        if (categoryTree.Exists(e => e.Id == parentId))
            return CategoryErrors.InvalidChildToMoving;

        var rootCategory = categoryTree.FirstOrDefault(e => e.Id == id)!;
        
        rootCategory.MoveCategory(parent.Id, parent.Path, parent.Level);

        var queue = new Queue<Category>();
        queue.Enqueue(rootCategory);
        
        var processed = new HashSet<Guid> { rootCategory.Id };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var children = categoryTree
                .Where(c => c.ParentId == current.Id && !processed.Contains(c.Id))
                .ToList();

            foreach (var child in children)
            {
                child.MoveCategory(current.Id, current.Path, current.Level);
                processed.Add(child.Id);
                queue.Enqueue(child);
            }
        }

        if (processed.Count != categoryTree.Count)
            return CategoryErrors.InconsistentTreeStructure;

        repository.UpdateRange(categoryTree);

        return categoryTree;
    }
    public async Task<Result> DeleteAsync(Guid id, Guid? parentId = null, CancellationToken ct = default)
    {
        if (await repository.FindAsync(id, ct: ct) is not { } category)
            return CategoryErrors.NotFound(id);

        if (await productCategoryRepository.ExistsAsync(e => e.CategoryId == id, ct: ct))
            return CategoryErrors.DeleteFailHasProducts;

        if (await repository.FindAllAsync(c => c.ParentId == id, ct: ct) is { } children)
        {
            if (parentId is null)
                return CategoryErrors.DeleteFailHasChildren;

            foreach (var child in children)
            {
                if (await MoveToNewParent(child.Id, parentId.Value, ct) is { IsFailure: true } moveError)
                    return moveError.Error;
            }
        }

        await categoryVariantRepository.ExecuteUpdateAsync(
            predicate: cv => cv.CategoryId == id,
            action: cv =>
            {
                cv.SetProperty(e => e.IsDeleted, true);
            }, ct);

        if (category.DeleteCategory() is { IsFailure: true } deleteError)
            return deleteError.Error;

        repository.Update(category);

        return Result.Success();
    }
    #endregion
    
    public async Task<Result> AddVariantAttributeToCategoryAsync(
        Guid id, 
        Guid variantId,
        short displayOrder,
        bool isRequired,
        CancellationToken ct = default)
    {
        if (!await repository.ExistsAsync(id, ct: ct))
            return CategoryErrors.NotFound(id);

        if (!await variantAttributeRepository.ExistsAsync(variantId, ct: ct))
            return VariantAttributeErrors.NotFound(variantId);

        if (await categoryVariantRepository.ExistsAsync(id, variantId, ct))
            return Errors.CategoryVariantAttributeErrors.AlreadyExists(id, variantId);

        var categoryVariant = CategoryVariantAttribute.Create(
            categoryId: id,
            variantAttributeId: variantId,
            isRequired: isRequired,
            displayOrder: displayOrder);

        if (categoryVariant.IsFailure)
            return categoryVariant.Error;

        categoryVariantRepository.Add(categoryVariant.Value!);

        return Result.Success();
    }
    public async Task<Result> AddBulkCategoryVariantAttributeAsync(
        Guid id,
        IEnumerable<(Guid variantId, bool isRequired, short displayOrder)> variants,
        CancellationToken ct = default)
    {
        if (!await repository.ExistsAsync(id, ct: ct))
            return CategoryErrors.NotFound(id);

        var categoryVariants = new List<CategoryVariantAttribute>();
        foreach (var variant in variants)
        {
            if (!await variantAttributeRepository.ExistsAsync(variant.variantId, ct: ct))
                return VariantAttributeErrors.NotFound(variant.variantId);

            if (await categoryVariantRepository.ExistsAsync(id, variant.variantId, ct))
                return CategoryVariantAttributeErrors.AlreadyExists(id, variant.variantId);

            var currentVariant = CategoryVariantAttribute.Create(
                categoryId: id,
                variantAttributeId: variant.variantId,
                isRequired: variant.isRequired,
                displayOrder: variant.displayOrder);

            if (currentVariant.IsFailure)
                return currentVariant.Error;

            categoryVariants.Add(currentVariant.Value!);
        }

        categoryVariantRepository.AddRange(categoryVariants);

        return Result.Success();
    }
    public async Task<Result> UpdateCategoryVariantAttributeAsync(
        Guid id, 
        Guid variantId, 
        short displayOrder,
        bool isRequired,
        CancellationToken ct = default)
    {
        if (await categoryVariantRepository.GetAsync(id, variantId, ct) is not { } categoryVariant)
            return CategoryVariantAttributeErrors.NotFound(id, variantId);

        
        categoryVariant.UpdateDisplayOrder(displayOrder);
        
        if (isRequired)
            categoryVariant.MarkRequired();
        else
            categoryVariant.MarkOptional();
        
        categoryVariantRepository.Update(categoryVariant);

        return Result.Success();
    }
    public async Task<Result> RemoveVariantAttributeFromCategoryAsync(
        Guid id,
        Guid variantId,
        CancellationToken ct = default)
    {
        if (await categoryVariantRepository.GetAsync(id, variantId, ct) is not { } categoryVariant)
            return CategoryVariantAttributeErrors.NotFound(id, variantId);

        categoryVariantRepository.Remove(categoryVariant);

        return Result.Success();
    }
    // TODO: Remove service
}
