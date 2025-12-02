namespace CatalogService.Domain.DomainService.Categories;

public sealed class CategoryDomainService(
    ICategoryRepository repository,
    IVariantAttributeRepository variantAttributeRepository,
    ICategoryVariantAttributeRepository categoryVariantRepository) : ICategoryDomainService
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
        if (await repository.ExistsAsync(e => e.Slug == slug, ct))
            return CategoryErrors.SlugAlreadyExist(slug);

        short level = 0;
        var path = "";
        if (parentId.HasValue && parentId.Value != Guid.Empty)
        {
            if (await repository.FindByIdAsync(parentId.Value, ct) is not { } parent)
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
            description: description);
    }

    public async Task<Result<List<Category>>> MoveToNewParent(
        Guid id, 
        Category parent, 
        CancellationToken ct = default)
    {
        var categoryTree = await repository.GetChildrenAsync(id, ct);

        if (categoryTree is null || categoryTree.Count == 0)
            return CategoryErrors.NotFound(id);

        if (categoryTree.Exists(e => e.Id == parent.Id))
            return CategoryErrors.InvalidChildToMoving;

        var rootCategory = categoryTree.Where(e => e.Id == id).FirstOrDefault()!;
        
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

        return categoryTree;
    }
    #endregion
    
    public async Task<Result> AddVariantAttributeToCategoryAsync(
        Guid id, 
        Guid variantId,
        short displayOrder,
        bool isRequired,
        CancellationToken ct = default)
    {
        if (!await repository.ExistsAsync(id, ct))
            return CategoryErrors.NotFound(id);

        if (!await variantAttributeRepository.ExistsAsync(variantId, ct))
            return VariantAttributeErrors.NotFound(variantId);

        if (await categoryVariantRepository.ExistsAsync(id, variantId, ct))
            return CategoryVariantAttributeErrors.AlreadyExists(id, variantId);

        var categoryVariant = CategoryVariantAttribute.Create(
            categoryId: id,
            variantAttributeId: variantId,
            isRequired: isRequired,
            displayOrder: displayOrder);

        categoryVariantRepository.Add(categoryVariant);

        return Result.Success();
    }
    public async Task<Result> AddBulkCategoryVariantAttributeAsync(
        Guid id,
        IEnumerable<(Guid variantId, bool isRequired, short displayOrder)> variants,
        CancellationToken ct = default)
    {
        if (!await repository.ExistsAsync(id, ct))
            return CategoryErrors.NotFound(id);

        foreach (var variant in variants)
        {
            if (!await variantAttributeRepository.ExistsAsync(variant.variantId, ct))
                return VariantAttributeErrors.NotFound(variant.variantId);

            if (await categoryVariantRepository.ExistsAsync(id, variant.variantId, ct))
                return CategoryVariantAttributeErrors.AlreadyExists(id, variant.variantId);
        }
        var categoryVariant = variants.Select(v =>
            CategoryVariantAttribute.Create(
                categoryId: id,
                variantAttributeId: v.variantId,
                isRequired: v.isRequired,
                displayOrder: v.displayOrder)
            );

        categoryVariantRepository.AddRange(categoryVariant);

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
