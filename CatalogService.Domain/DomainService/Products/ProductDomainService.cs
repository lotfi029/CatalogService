using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.DomainService.Products;

public sealed class ProductDomainService(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
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
        if (await productRepository.FindByIdAsync(id, ct) is not { } product)
            return ProductErrors.NotFound(id);

        product.UpdateDetails(name: name, description: description);

        productRepository.Update(product);
        return Result.Success();
    }
    public async Task<Result> UpdateProductStatus(
        Guid id,
        string status,
        CancellationToken ct = default)
    {
        if (await productRepository.FindByIdAsync(id, ct) is not { } product)
            return ProductErrors.NotFound(id);

        if (!Enum.TryParse<ProductStatus>(status, ignoreCase: true, out var productStatus))
            return ProductErrors.InvalidProductStatus(status);

        var statusResult = productStatus switch
        {
            ProductStatus.Active => product.Activate(),
            ProductStatus.Archive => product.Archive(),
            ProductStatus.Inactive => product.Inactivate(),
            ProductStatus.Draft => product.Draft(),
            _ => ProductErrors.InvalidProductStatus(status)
        };

        if (statusResult.IsFailure)
            return statusResult;

        productRepository.Update(product);
        return Result.Success();
    }
    public async Task<Result> ActivaAsync(Guid productId, CancellationToken ct = default)
    {
        if (await productRepository.FindByIdAsync(productId, ct) is not { } product)
            return ProductErrors.NotFound(productId);

        switch(product.Status)
        {
            case ProductStatus.Active:
                return ProductErrors.ProductAlreadyActive;
            case ProductStatus.Archive:
                return ProductErrors.ProductIsArchived;
            case ProductStatus.Draft:
                if (await productCategoryRepository.ExistsAsync(pc => pc.ProductId == productId, ct: ct))
                    return ProductErrors.InvlalidActivateProcess;
                if (await productVariantRepository.ExistsAsync(pc => pc.ProductId == productId, ct: ct))
                    return ProductErrors.InvlalidActivateProcess;
                if (await productAttributeRepository.ExistsAsync(pc => pc.ProductId == productId, ct: ct))
                    return ProductErrors.InvlalidActivateProcess;
                break;
            default:
                break;
        }

        product.Activate();
        productRepository.Update(product);
        return Result.Success();
    }

    public async Task<Result> UpdateProductVariantCustomizationOptionsAsync(Guid id,ProductVariantsOption customOption, CancellationToken ct = default)
    {
        if (await productVariantRepository.GetById(id: id, ct) is not { } productVariant)
            return ProductVariantErrors.NotFound(id);

        if (productVariant.UpdateCustomizationOptions(customOption) is { IsFailure: true } updatingError)
            return updatingError.Error;

        productVariantRepository.Update(productVariant);

        return Result.Success();
    }
    public async Task<Result> UpdateProductVariantPriceAsync(Guid id, decimal price, decimal? compareAtPrice, string currency, CancellationToken ct = default)
    {
        if (await productVariantRepository.GetById(id: id, ct) is not { } productVariant)
            return ProductVariantErrors.NotFound(id);

        currency = currency.ToUpper();

        if (productVariant.UpdatePrice(price, compareAtPrice, currency) is { IsFailure: true } updatingError)
            return updatingError.Error;
        
        productVariantRepository.Update(productVariant);
        return Result.Success();
    }
    public async Task<Result> DeleteProductVariantAsync(Guid id, CancellationToken ct = default)
    {
        var deletedRaws = await productVariantRepository.ExecuteDeleteAsync(x => x.Id == id, ct: ct);

        return deletedRaws == 0
            ? ProductVariantErrors.NotFound(id)
            : Result.Success();
    }
    public async Task<Result> DeleteAllProductVariantAsync(Guid productId, CancellationToken ct = default)
    {
        var deletedRaws = await productVariantRepository.ExecuteDeleteAsync(x => x.ProductId == productId, ct: ct);

        return Result.Success();
    }
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

        if (await productRepository.ExistsAsync(productId, ct) is false)
            return ProductErrors.NotFound(productId);

        if (await categoryRepository.ExistsAsync(categoryId, ct) is false)
            return CategoryErrors.NotFound(categoryId);

        var categoryVariants = await categoryVariantRepository
            .GetCategoryVariantIncludeVariantsId(categoryId, ct) 
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
    public async Task<Result> UpdateProductCategory(Guid productId, Guid categoryId, bool isPrimary, CancellationToken ct = default)
    {
        if (await productCategoryRepository.GetAsync(productId, categoryId, ct) is not { } productCategory)
            return ProductCategoriesErrors.NotFound;

        if (await productRepository.FindByIdAsync(productId, ct) is not { } product)
            return ProductErrors.NotFound(productId);

        if (product.UpdateCategory(productCategory, isPrimary) is { IsFailure: true } updateResult)
            return updateResult.Error;

        return Result.Success();
    }
    public async Task<Result> RemoveCategory(Guid productId, Guid categoryId, CancellationToken ct = default)
    {
        if (await productCategoryRepository.GetAsync(productId, categoryId, ct) is not { } productCategory)
            return ProductCategoriesErrors.NotFound;

        if (await productRepository.FindByIdAsync(productId, ct) is not { } product)
            return ProductErrors.NotFound(productId);

        if (product.RemoveCategory(categoryId) is { IsFailure: true} removeResult)
            return removeResult.Error;

        productCategoryRepository.Remove(productCategory);

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

        productCategoryRepository.Add(productCategory);

        if (addedProductVariant.Count > 0)
        {
            productVariantRepository.AddRange([.. addedProductVariant]);
        }

        return Result.Success();
    }
    #endregion
}