namespace CatalogService.Domain.DomainService.Products;

public sealed class ProductDomainService(
    IProductRepository productRepository) : IProductDomainService
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
}