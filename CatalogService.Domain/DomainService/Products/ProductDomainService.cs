using CatalogService.Domain.DomainEvents.Products.ProductAttributes;
using CatalogService.Domain.DomainEvents.Products.ProductCategories;
using CatalogService.Domain.DomainEvents.Products.ProductVariants;
using System.Diagnostics.Tracing;

namespace CatalogService.Domain.DomainService.Products;

public sealed class ProductDomainService(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IAttributeRepository attributeRepository,
    ICategoryVariantAttributeRepository categoryVariantRepository,
    IProductVariantRepository productVariantRepository,
    IProductAttributeRepository productAttributeRepository,
    IProductVariantValueRepository productVariantValueRepository,
    IProductCategoryRepository productCategoryRepository) : IProductDomainService
{
    #region product
    public Result<Guid> Create(
        Guid vendorId,
        string name,
        string? description)
    {
        var product = Product.Create(
            name: name,
            description: description,
            vendorId: vendorId);

        if (product.IsFailure)
            return product.Error;

        productRepository.Add(product.Value!);

        return product.Value!.Id;
    }
    public Result CreateBulk(
        Guid vendorId,
        IEnumerable<(string name, string? description)> values)
    {
        List<Product> products = [];
        foreach (var (name, description) in values)
        {
            var product = Product.Create(
                name: name,
                description: description,
                vendorId: vendorId);

            if (product.IsFailure)
                return product.Error;

            products.Add(product.Value!);
        }

        productRepository.AddRange(products);
        return Result.Success();
    }
    public async Task<Result> UpdateDetails(
        Guid userId,
        Guid id,
        string name,
        string? description,
        CancellationToken ct = default)
    {
        if (await productRepository.FindAsync(id, null, ct) is not { } product)
            return ProductErrors.NotFound(id);

        if (product.VendorId != userId)
            return ProductErrors.InvalidAccess;

        if (product.Status == ProductStatus.Archive)
            return ProductErrors.ProductIsArchived;

        product.UpdateDetails(name: name, description: description);

        productRepository.Update(product);
        return Result.Success();
    }
    public async Task<Result> ActivateAsync(Guid userId, Guid productId, CancellationToken ct = default)
    {
        if (await productRepository.FindAsync(productId, null, ct) is not { } product)
            return ProductErrors.NotFound(productId);

        if (product.VendorId != userId)
            return ProductErrors.InvalidAccess;

        switch (product.Status)
        {
            case ProductStatus.Active:
                return ProductErrors.ProductAlreadyActive;
            case ProductStatus.Archive:
                return ProductErrors.ProductAlreadyArchived;
            case ProductStatus.Draft:
                if (!await productCategoryRepository.ExistsAsync(pc => pc.ProductId == productId, ct: ct))
                    return ProductErrors.InvlalidActivateProcess;
                if (!await productVariantRepository.ExistsAsync(pc => pc.ProductId == productId, ct: ct))
                    return ProductErrors.InvlalidActivateProcess;
                if (!await productAttributeRepository.ExistsAsync(pc => pc.ProductId == productId, ct: ct))
                    return ProductErrors.InvlalidActivateProcess;
                break;
            default:
                break;
        }

        product.Activate();
        productRepository.Update(product);
        return Result.Success();
    }
    public async Task<Result> ArchiveAsync(Guid userId, Guid productId, CancellationToken ct = default)
    {
        if (await productRepository.FindAsync(productId, null, ct) is not { } product)
            return ProductErrors.NotFound(productId);

        if (product.VendorId != userId)
            return ProductErrors.InvalidAccess;

        if (product.Status == ProductStatus.Archive)
            return ProductErrors.ProductAlreadyArchived;

        product.Archive();
        productRepository.Update(product);
        return Result.Success();
    }
    #endregion

    #region product variants
    public async Task<Result> AddProductVariantAsync(
        Guid userId,
        Guid productId,
        decimal price,
        decimal? compareAtPrice,
        Dictionary<Guid, string> variantAttributes,
        CancellationToken ct = default)
    {
        if (await ValidateProductOwnership(userId, productId, ct) is { IsFailure: true } validationErrors)
            return validationErrors.Error;

        var variantResult = await ProductVariantValidationAsync(
            productId: productId,
            variantAttributes,
            ct);

        if (variantResult.IsFailure)
            return variantResult.Error;

        var productVariant = ProductVariant.Create(
            productId: productId,
            variantResult.Value!,
            price: new Money(price),
            compareAtPrice: compareAtPrice.HasValue ? new Money(compareAtPrice) : null
            );

        var values = variantAttributes
            .Select(va =>
                ProductVariantValue.Create(
                    productVariantId: productVariant.Id,
                    va.Key,
                    va.Value
                    ));

        productVariantRepository.Add(productVariant);
        productVariantValueRepository.AddRange([.. values]);

        return Result.Success();
    }

    public async Task<Result> UpdateProductVariantPriceAsync(
        Guid userId, 
        Guid variantId, 
        decimal price, 
        decimal? compareAtPrice, 
        string currency, 
        CancellationToken ct = default)
    {

        if (await productVariantRepository.GetById(id: variantId, ct) is not { } productVariant)
            return ProductVariantErrors.NotFound(variantId);

        if (await ValidateProductOwnership(userId, productVariant.ProductId, ct) is { IsFailure: true } validationError)
            return validationError;

        if (productVariant.UpdatePrice(price, compareAtPrice, currency) is { IsFailure: true } error)
            return error;

        AddDomainEvents(
            productVariant.ProductId, 
            new ProductVariantUpdatedDomainEvent(productVariant.ProductId, variantId)
        );
        return Result.Success();
    }
    
    public async Task<Result> DeleteProductVariantAsync(
        Guid userId, 
        Guid productId, 
        Guid productVariantId, 
        CancellationToken ct = default)
    {
        if (await ValidateProductOwnership(userId, productId, ct) is { IsFailure: true } validationError)
            return validationError;

        var updatedRows = await productVariantRepository.
            ExecuteUpdateAsync(
                predicate: x => x.Id == productVariantId,
                action: setter => setter
                    .SetProperty(pv => pv.IsDeleted, true),
                ct);

        if (updatedRows == 0)
            return ProductVariantErrors.NotFound(productVariantId);

        await productVariantValueRepository.ExecuteUpdateAsync(
            predicate: x => x.ProductVariantId == productVariantId,
            action: setter => setter
                .SetProperty(x => x.IsDeleted, true),
            ct);

        AddDomainEvents(
            productId, 
            new ProductVariantDeletedDomainEvent(productId,productVariantId)
        );

        return Result.Success();
    }
    #endregion
    
    #region product category
    public async Task<Result> AddProductCategory(
        Guid userId,
        Guid productId,
        Guid categoryId,
        bool isPrimary,
        CancellationToken ct = default)
    {
        if (await ValidateProductOwnership(userId, productId, ct) is { IsFailure: true } validationError)
            return validationError;

        if (await productCategoryRepository.ExistsAsync(productId, categoryId, ct))
            return ProductCategoriesErrors.DuplicatedCategory(categoryId);

        if (!await categoryRepository.ExistsAsync(categoryId, ct: ct))
            return CategoryErrors.NotFound(categoryId);

        var productCategory = ProductCategories.Create(
            productId: productId,
            categoryId: categoryId,
            isPrimary: isPrimary);

        productCategoryRepository.Add(productCategory);
        AddDomainEvents(
            productId,
            new ProductCategoryAddedDomainEvent(productId, categoryId)
        );
        return Result.Success();
    }
    public async Task<Result> UpdateProductCategoryAsync(
        Guid userId, 
        Guid productId, 
        Guid categoryId, 
        bool isPrimary, 
        CancellationToken ct = default)
    {
        if (await ValidateProductOwnership(userId, productId, ct) is { IsFailure: true } validationError)
            return validationError;

        var rowAffected = await productCategoryRepository.ExecuteUpdateAsync(
            predicate: pc => pc.CategoryId == categoryId && pc.ProductId == productId,
            action: action =>
            {
                action.SetProperty(pc => pc.IsPrimary, isPrimary);
            },
            ct: ct);

        if (rowAffected == 0)
            return ProductCategoriesErrors.NotFound;

        AddDomainEvents(
            productId, 
            new ProductCategoryUpdatedDomainEvent(productId, categoryId)
        );

        return Result.Success();
    }
    public async Task<Result> DeleteCategoryAsync(
        Guid userId, 
        Guid productId, 
        Guid categoryId, 
        CancellationToken ct = default)
    {
        if (await ValidateProductOwnership(userId, productId, ct) is { IsFailure: true } validationError)
            return validationError;

        var rowAffected = await productCategoryRepository.ExecuteUpdateAsync(
            predicate: pc => pc.ProductId == productId && pc.CategoryId == categoryId,
            action: body => body
                .SetProperty(prop => prop.IsDeleted, true),
            ct: ct);

        if (rowAffected == 0)
            return ProductCategoriesErrors.NotFound;

        // delete all variant in this category from this product
        var variantIds = await categoryVariantRepository
            .GetVariantAttributeIds(categoryId, ct);
        // validate soft delete
        if (variantIds.Count != 0)
        {
            var productVariantIds = await productVariantValueRepository
                .GetProductVariantIdsAsync(x => variantIds.Contains(x.VariantAttributeId), ct);

            await productVariantRepository.ExecuteUpdateAsync(
                predicate: x => productVariantIds.Contains(x.Id),
                action: setter => setter
                    .SetProperty(prop => prop.IsDeleted, true),
                ct);

            await productVariantValueRepository.ExecuteUpdateAsync(
                predicate: x => variantIds.Contains(x.VariantAttributeId),
                action: setter => setter
                    .SetProperty(x => x.IsDeleted, true),
                ct);
        }

        AddDomainEvents(
            productId, 
            new ProductCategoryRemovedDomainEvent(productId, categoryId)
        );
        return Result.Success();
    }
    #endregion
   
    private async Task<Result<List<string>>> ProductVariantValidationAsync(
        Guid productId, 
        Dictionary<Guid, string> variantAttributes, 
        CancellationToken ct = default)
    {
        var categories = await productCategoryRepository
                .GetCategoryIdsAsync(productId, ct);

        if (categories.Count == 0)
            return ProductVariantErrors.InvalidVariants;

        var variantIds = new HashSet<Guid>(variantAttributes.Keys);
        var categoryVariants = await categoryVariantRepository
            .GetAsync(categories, variantIds, ct)
            ?? [];

        if (variantIds.Except(categoryVariants.Select(e => e.VariantAttributeId)).Any())
            return ProductVariantErrors.InvalidVariants;

        var variantsLookup = categoryVariants
            .Join(
                inner: variantAttributes,
                outerKeySelector: cva => cva.VariantAttributeId,
                innerKeySelector: va => va.Key,
                (cva, va) => new 
                {
                    cva.VariantAttributeId,
                    cva.DisplayOrder,
                    cva.VariantAttribute.Code,
                    AllowedValues = cva.VariantAttribute.AllowedValues!.Values,
                    cva.VariantAttribute.Datatype.DataType,
                    va.Value
                })
            .OrderBy(x => x.DisplayOrder)
            .ToDictionary(keySelector: k => k.VariantAttributeId);

        List<string> sku = [];
        foreach(var variant in variantsLookup)
        {
            var validationResult = ValidateVariantValue(
                variant.Value.AllowedValues, 
                variant.Value.DataType, 
                variant.Value.Code, 
                variant.Value.Value);

            if (validationResult.IsFailure)
                return validationResult.Error;

            sku.Add(variant.Value.Value);
        }
        return Result.Success(sku);
    }
    private static Result ValidateVariantValue(
        HashSet<string>? AllowedValues, 
        VariantDataType Datatype,
        string code,
        string value)
    {
        switch (Datatype)
        {
            case VariantDataType.Select:
                var allowedValues = AllowedValues!;

                if (!allowedValues.Contains(value, comparer: StringComparer.OrdinalIgnoreCase))
                {
                    return ProductVariantErrors.InvalidVariantValue(code, value, allowedValues);
                }
                break;
            case VariantDataType.Boolean:
                if (!bool.TryParse(value, out bool _))
                {
                    return ProductVariantErrors.InvalidBooleanValue(code, value);
                }
                break;

            default:
                break;
        }
        return Result.Success();
    }
    #region product attribute 
    public async Task<Result> AddAttributeAsync(Guid userId, Guid productId, Guid attributeId, string value, CancellationToken ct = default)
    {
        if (await productAttributeRepository.ExistsAsync(productId: productId, attributeId: attributeId, ct))
            return ProductAttributeErrors.DuplicatedAttribute;

        if (!await productRepository.ExistsAsync(e => e.Id == productId && e.VendorId == userId, ct: ct))
            return ProductErrors.NotFound(productId);

        if (await attributeRepository.FindAsync(attributeId, null, ct) is not { } attribute)
            return AttributeErrors.NotFound(attributeId);

        var map = new Dictionary<Entities.Attribute, string>
        {
            { attribute, value }
        };

        if (ValidAttributeValue(map) is { IsFailure: true } validationErrors)
            return validationErrors.Error;

        var productAttribute = ProductAttributes.Create(
            productId: productId,
            attributeId: attributeId,
            value: value);
        productAttributeRepository.Add(productAttribute);

        AddDomainEvents(productId, new ProductAttributeAddedDomainEvent(productId, attributeId));
        return Result.Success();
    }
    public async Task<Result> AddAttributeBulkAsync(Guid userId, Guid productId, IEnumerable<(Guid attributeId, string value)> values, CancellationToken ct = default)
    {
        if (!await productRepository.ExistsAsync(e => e.Id == productId && e.VendorId == userId, ct: ct))
            return ProductErrors.NotFound(productId);

        var attributeIds = values
            .Select(e => e.attributeId)
            .ToList();

        if (!await productAttributeRepository.ExistsAsync(pa => attributeIds.Contains(pa.AttributeId) && pa.ProductId == productId, ct))
            return ProductAttributeErrors.DuplicatedAttribute;

        if (await attributeRepository.FindAllAsync(e => attributeIds.Contains(e.Id), ct) is not { } attributes)
            return AttributeErrors.NotFound(Guid.Empty);

        var map = attributes.Join(
            values,
            a => a.Id,
            v => v.attributeId,
            (a, v) => (a, v.value))
            .ToDictionary(k => k.a, v => v.value);

        if (ValidAttributeValue(map) is { IsFailure: true } validationErrors)
            return validationErrors.Error;

        var productAttributeList = values.Select(e => ProductAttributes.Create(
            productId: productId,
            attributeId: e.attributeId,
            value: e.value
            ));

        productAttributeRepository.AddRange([.. productAttributeList]);

        AddDomainEvents(productId, new ProductAttributeAddedBulkDomainEvent(productId));

        return Result.Success();
    }
    private static Result ValidAttributeValue(Dictionary<Entities.Attribute, string> attributes)
    {
        foreach (var i in attributes)
        {
            if (i.Key.OptionsType.DataType == VariantDataType.Select)
            {
                var options = i.Key.Options!.Values;
                if (!options.Contains(i.Value, StringComparer.OrdinalIgnoreCase))
                    return ProductAttributeErrors.InvalidAttributeValue(i.Key.Name, i.Value, options);
            }
            else if (i.Key.OptionsType.DataType == VariantDataType.Boolean)
            {
                if (!bool.TryParse(i.Value, out var _))
                    return ProductAttributeErrors.InvalidBooleanValue(i.Key.Name, i.Value);
            }
        }

        return Result.Success();
    }
    public async Task<Result> UpdateAttributeValueAsync(Guid userId, Guid productId, Guid attributeId, string newValue, CancellationToken ct = default)
    {
        if (await productRepository.ExistsAsync(e => e.Id == productId && e.VendorId == userId, ct: ct))
            return ProductErrors.InvalidAccess;
        var productAttribute = await productAttributeRepository.GetById(productId: productId, attributeId: attributeId, ct);
        if (productAttribute is null)
            return ProductAttributeErrors.NotFound(productId, attributeId);

        if (productAttribute.UpdateValue(newValue) is { IsFailure: true } updatingError)
            return updatingError.Error;
        productAttributeRepository.Update(productAttribute);

        
        AddDomainEvents(productId, new ProductAttributeUpdatedDomainEvent(productId, attributeId));
        return Result.Success();
    }
    public async Task<Result> DeleteAttributeAsync(Guid userId, Guid productId, Guid attributeId, CancellationToken ct = default)
    {
        if (await productRepository.ExistsAsync(e => e.Id == productId && e.VendorId == userId, ct: ct))
            return ProductErrors.InvalidAccess;

        var deletedRaws = await productAttributeRepository
            .ExecuteDeleteAsync(e => e.ProductId == productId && e.AttributeId == attributeId, ct);

        AddDomainEvents(productId, new ProductAttributeDeletedDomainEvent(productId, attributeId));
        
        return deletedRaws == 0
            ? ProductAttributeErrors.NotFound(productId, attributeId)
            : Result.Success();
    }
    public async Task<Result> DeleteAllAttributeAsync(Guid userId, Guid productId, CancellationToken ct = default)
    {
        if (await productRepository.ExistsAsync(e => e.Id == productId && e.VendorId == userId, ct: ct))
            return ProductErrors.InvalidAccess;

        var deletedRaws = await productAttributeRepository
            .ExecuteDeleteAsync(e => e.ProductId == productId, ct);

        AddDomainEvents(productId, new ProductAttributeDeletedAllDomainEvent(productId));
        return Result.Success();
    }
    #endregion
    private async Task<Result> ValidateProductOwnership(
        Guid userId,
        Guid productId,
        CancellationToken ct)
    {
        if (!await productRepository.ExistsAsync(
            e => e.Id == productId && e.VendorId == userId, ct: ct))
            return ProductErrors.InvalidAccess;

        return Result.Success();
    }
    private void AddDomainEvents(Guid ProductId, IDomainEvent domainEvent)
    {
        var proxyProduct = Product.CreateProxy(ProductId);
        productRepository.Attach(proxyProduct);
        proxyProduct.AddDomainEvent(domainEvent);
    }
}