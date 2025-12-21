using CatalogService.Domain.DomainEvents.Products;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.DomainService.Products;

public sealed class ProductDomainService(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IAttributeRepository attributeRepository,
    ICategoryVariantAttributeRepository categoryVariantRepository,
    IProductVariantRepository productVariantRepository,
    IProductAttributeRepository productAttributeRepository,
    IProductCategoryRepository productCategoryRepository) : IProductDomainService
{
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
        Guid id,
        string name,
        string? description,
        CancellationToken ct = default)
    {
        if (await productRepository.FindAsync(id, null, ct) is not { } product)
            return ProductErrors.NotFound(id);

        if (product.Status == ProductStatus.Archive)
            return ProductErrors.ProductIsArchived;

        product.UpdateDetails(name: name, description: description);

        productRepository.Update(product);
        return Result.Success();
    }
    public async Task<Result> ActivaAsync(Guid productId, CancellationToken ct = default)
    {
        if (await productRepository.FindAsync(productId, null, ct) is not { } product)
            return ProductErrors.NotFound(productId);

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
    public async Task<Result> ArchiveAsync(Guid productId, CancellationToken ct = default)
    {
        if (await productRepository.FindAsync(productId, null, ct) is not { } product)
            return ProductErrors.NotFound(productId);

        if (product.Status == ProductStatus.Archive)
            return ProductErrors.ProductAlreadyArchived;

        product.Archive();
        productRepository.Update(product);
        return Result.Success();
    }
    #region product variants
    public async Task<Result> UpdateProductVariantCustomizationOptionsAsync(Guid variantId, ProductVariantsOption customOption, CancellationToken ct = default)
    {
        if (await productVariantRepository.GetById(id: variantId, ct) is not { } productVariant)
            return ProductVariantErrors.NotFound(variantId);

        if (productVariant.UpdateCustomizationOptions(customOption) is { IsFailure: true } updatingError)
            return updatingError.Error;

        productVariantRepository.Update(productVariant);

        AddDomainEvents(productVariant.ProductId, new ProductVariantUpdatedDomainEvent(productVariant.ProductId, variantId));
        return Result.Success();
    }
    public async Task<Result> UpdateProductVariantPriceAsync(Guid variantId, decimal price, decimal? compareAtPrice, string currency, CancellationToken ct = default)
    {
        if (await productVariantRepository.GetById(id: variantId, ct) is not { } productVariant)
            return ProductVariantErrors.NotFound(variantId);

        currency = currency.ToUpper();

        if (productVariant.UpdatePrice(price, compareAtPrice, currency) is { IsFailure: true } updatingError)
            return updatingError.Error;

        productVariantRepository.Update(productVariant);
        AddDomainEvents(productVariant.ProductId, new ProductVariantUpdatedDomainEvent(productVariant.ProductId, variantId));
        return Result.Success();
    }
    public async Task<Result> DeleteProductVariantAsync(Guid productId, Guid variantId, CancellationToken ct = default)
    {
        var deletedRaws = await productVariantRepository.ExecuteDeleteAsync(x => x.Id == variantId, ct: ct);

        AddDomainEvents(productId, new ProductVariantDeletedDomainEvent(productId,variantId));
        return deletedRaws == 0
            ? ProductVariantErrors.NotFound(variantId)
            : Result.Success();
    }
    public async Task<Result> DeleteAllProductVariantAsync(Guid productId, CancellationToken ct = default)
    {
        var deletedRaws = await productVariantRepository.ExecuteDeleteAsync(x => x.ProductId == productId, ct: ct);
        AddDomainEvents(productId, new ProductVariantDeletedAllDomainEvent(productId));
        return Result.Success();
    }
    #endregion
    #region product category
    public async Task<Result> AddProductCategory(
        Guid productId,
        Guid categoryId,
        bool isPrimary,
        List<(decimal price, decimal? compareAtPrice, ProductVariantsOption variants)> productVariants,
        CancellationToken ct = default)
    {
        if (await productCategoryRepository.ExistsAsync(productId, categoryId, ct))
            return ProductCategoriesErrors.DuplicatedCategory(categoryId);

        if (await productRepository.ExistsAsync(productId, ct: ct) is false)
            return ProductErrors.NotFound(productId);

        if (await categoryRepository.ExistsAsync(categoryId, ct: ct) is false)
            return CategoryErrors.NotFound(categoryId);

        var categoryVariants = await categoryVariantRepository
            .GetCategoryVariantsByCategoryIdId(categoryId, ct)
            ?? [];

        var variantsLookup = categoryVariants
            .OrderBy(cv => cv.DisplayOrder)
            .ToDictionary(
            keySelector: v => v.VariantAttribute.Code,
            elementSelector: v => new
            {
                v.DisplayOrder,
                v.VariantAttribute.Datatype.DataType,
                v.VariantAttribute.AffectsInventory,
                v.VariantAttribute.Code,
                AllowedValues = v.VariantAttribute.AllowedValues?.Values ?? []
            },
            comparer: StringComparer.OrdinalIgnoreCase);

        List<ProductVariant> addedProductVariant = [];

        foreach (var (price, compareAtPrice, variants) in productVariants)
        {
            var inputVariants = variants.Variants;
            var inputCodes = inputVariants.Select(e => e.Key);

            if (!variantsLookup.Keys.All(inputCodes.Contains))
            {
                return ProductCategoriesErrors.InvalidIncludedVariants([.. variantsLookup.Keys]);
            }
            List<VariantAttributeItem> variantAttributeItems = [];
            List<VariantAttributeItem> customizedOptions = [];
            foreach (var (code, value) in variants.Variants)
            {
                if (variantsLookup.TryGetValue(code, out var requiredVarian))
                {
                    if (validateVariantValue((requiredVarian.AllowedValues, requiredVarian.DataType), code, value) is { IsFailure: true } valueValidationError)
                        return valueValidationError.Error;

                    if (requiredVarian.AffectsInventory)
                    {
                        variantAttributeItems.Add(new(requiredVarian.Code, value));
                    }
                    else
                    {
                        customizedOptions.Add(new(requiredVarian.Code, value));
                    }
                }
                else
                {
                    customizedOptions.Add(new(code, value));
                }
            }
            addedProductVariant.Add(ProductVariant.Create(
                productId: productId,
                variantAttributes: new(variantAttributeItems),
                customizationOptions: new(customizedOptions),
                price: new(price),
                compareAtPrice: new(compareAtPrice)
                ));
        }

        var finalizeAdditionResult = finalizeProductCategoryAddition(productId, categoryId, isPrimary, addedProductVariant);
        if (finalizeAdditionResult.IsFailure)
            return finalizeAdditionResult.Error;



        return Result.Success();
    }
    public async Task<Result> UpdateProductCategoryAsync(Guid productId, Guid categoryId, bool isPrimary, CancellationToken ct = default)
    {
        if (await productCategoryRepository.GetAsync(productId, categoryId, ct) is not { } productCategory)
            return ProductCategoriesErrors.NotFound;

        await productCategoryRepository.ExecuteUpdateAsync(
            predicate: pc => pc.CategoryId == categoryId && pc.ProductId == productId,
            action: action =>
            {
                action.SetProperty(pc => pc.IsPrimary, isPrimary);
            },
            ct: ct);

        AddDomainEvents(productId, new ProductCategoryUpdatedDomainEvent(productId, categoryId));

        return Result.Success();
    }
    public async Task<Result> RemoveCategory(Guid productId, Guid categoryId, CancellationToken ct = default)
    {
        if (await productCategoryRepository.GetAsync(productId, categoryId, ct) is not { } productCategory)
            return ProductCategoriesErrors.NotFound;

        productCategoryRepository.Remove(productCategory);
        AddDomainEvents(productId, new ProductCategoryRemovedDomainEvent(productId, categoryId));
        return Result.Success();
    }
    private Result validateVariantValue(
        (HashSet<string>? AllowedValues, VariantDataType Datatype) requiredVariant,
        string code,
        string value)
    {
        switch (requiredVariant.Datatype)
        {
            case VariantDataType.Select:
                var allowedValues = requiredVariant.AllowedValues!;

                if (!allowedValues.Contains(value, comparer: StringComparer.OrdinalIgnoreCase))
                {
                    return ProductVariantErrors.InvalidVariantValue(code, value, allowedValues);
                }
                break;
            case VariantDataType.Boolean:
                if (!bool.TryParse(code, out bool _))
                {
                    return ProductVariantErrors.InvalidBooleanValue(code, value);
                }
                break;

            default:
                break;
        }
        return Result.Success();
    }

    private Result finalizeProductCategoryAddition(
        Guid productId,
        Guid categoryId,
        bool isPrimary,
        List<ProductVariant> addedProductVariant)
    {
        var productCategory = ProductCategories.Create(
            productId: productId,
            categoryId: categoryId,
            isPrimary: isPrimary);

        if (productCategory.IsFailure)
            return productCategory.Error;

        productCategoryRepository.Add(productCategory.Value!);

        if (addedProductVariant.Count > 0)
        {
            productVariantRepository.AddRange([.. addedProductVariant]);
        }

        AddDomainEvents(productId, new ProductCategoryAddedDomainEvent(productId, categoryId));

        return Result.Success();
    }
    #endregion
    #region product attribute 
    public async Task<Result> AddAttributeAsync(Guid productId, Guid attributeId, string value, CancellationToken ct = default)
    {
        if (await productAttributeRepository.ExistsAsync(productId: productId, attributeId: attributeId, ct))
            return ProductAttributeErrors.DuplicatedAttribute;

        if (!await productRepository.ExistsAsync(productId, ct: ct))
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
    public async Task<Result> AddAttributeBulkAsync(Guid productId, IEnumerable<(Guid attributeId, string value)> values, CancellationToken ct = default)
    {
        if (!await productRepository.ExistsAsync(productId, ct: ct))
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
    public async Task<Result> UpdateAttributeValueAsync(Guid productId, Guid attributeId, string newValue, CancellationToken ct = default)
    {
        var productAttribute = await productAttributeRepository.GetById(productId: productId, attributeId: attributeId, ct);
        if (productAttribute is null)
            return ProductAttributeErrors.NotFound(productId, attributeId);

        if (productAttribute.UpdateValue(newValue) is { IsFailure: true } updatingError)
            return updatingError.Error;
        productAttributeRepository.Update(productAttribute);

        
        AddDomainEvents(productId, new ProductAttributeUpdatedDomainEvent(productId, attributeId));
        return Result.Success();
    }
    public async Task<Result> DeleteAttributeAsync(Guid productId, Guid attributeId, CancellationToken ct = default)
    {
        var deletedRaws = await productAttributeRepository
            .ExecuteDeleteAsync(e => e.ProductId == productId && e.AttributeId == attributeId, ct);

        AddDomainEvents(productId, new ProductAttributeDeletedDomainEvent(productId, attributeId));
        
        return deletedRaws == 0
            ? ProductAttributeErrors.NotFound(productId, attributeId)
            : Result.Success();
    }
    public async Task<Result> DeleteAllAttributeAsync(Guid productId, CancellationToken ct = default)
    {
        var deletedRaws = await productAttributeRepository
            .ExecuteDeleteAsync(e => e.ProductId == productId, ct);

        AddDomainEvents(productId, new ProductAttributeDeletedAllDomainEvent(productId));
        return Result.Success();
    }
    #endregion

    private static void AddDomainEvents(Guid ProductId, IDomainEvent domainEvent)
    {
        var proxyProduct = Product.CreateProxy(ProductId);
        proxyProduct.AddDomainEvent(domainEvent);
    }
}