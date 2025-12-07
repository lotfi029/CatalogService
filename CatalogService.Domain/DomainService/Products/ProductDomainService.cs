using System.Runtime.CompilerServices;

namespace CatalogService.Domain.DomainService.Products;

public sealed class ProductDomainService(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    ICategoryVariantAttributeRepository categoryVariantRepository,
    IProductVariantRepository productVariantRepository,
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
    //public async Task<Result> ActiveAysnc(Guid id, CancellationToken ct = default)
    //{
    //    if (await productRepository.FindByIdAsync(id, ct) is not { } product)
    //        return ProductErrors.NotFound(id);

    //    if (await productCategoryRepository.GetByProductIdAsync(id, ct) is not { } productCategories)
    //        return ProductErrors.CategoriesNotFound;

    //    var productVariants = await productVariantRepository.GetWithPredicateAsync(pv => pv.ProductId == id, ct);

    //    foreach (var productCategory in productCategories)
    //    {
    //        var categoryVariants = await categoryVariantRepository.GetByCategoryIdAsync(productCategory.CategoryId, ct);
            
    //        if (categoryVariants is null || !categoryVariants.Any())
    //            continue;

    //    }
            
    //}
    public async Task<Result> AddProductCategory(Guid productId, Guid categoryId, bool isPrimary, CancellationToken ct = default)
    {
        if (await productCategoryRepository.ExistsAsync(productId, categoryId, ct))
            return ProductCategoriesErrors.DuplicatedCategory(categoryId);

        if (await productRepository.FindByIdAsync(productId, ct) is not { } product)
            return ProductErrors.NotFound(productId);

        if (await categoryRepository.ExistsAsync(categoryId, ct) is false)
            return CategoryErrors.NotFound(categoryId);

        
        var productCategory = ProductCategories.Create(
            productId: productId,
            categoryId: categoryId,
            isPrimary: isPrimary);

        product.AddCategory(categoryId);
        productCategoryRepository.Add(productCategory);

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

    //public async Task<Result> AddCategoryVariantsAsync(Guid productId, Guid categoryId, CancellationToken ct = default)
    //{
    //    if (!await productCategoryRepository.ExistsAsync(productId: productId, categoryId: categoryId, ct))
    //        return ProductCategoriesErrors.NotFound;

    //    if (await categoryVariantRepository.GetByCategoryIdAsync(categoryId, ct) is not { } categoryVariants)
    //        return CategoryVariantAttributeErrors.VariantNotFound(categoryId);


    //}
}