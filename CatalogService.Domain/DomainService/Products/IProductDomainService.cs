using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.DomainService.Products;

public interface IProductDomainService
{
    Task<Result> ActivaAsync(Guid productId, CancellationToken ct = default);
    Task<Result> AddProductCategory(Guid productId, Guid categoryId, bool isPrimary, List<(decimal price, decimal? compareAtPrice, ProductVariantsOption variants)> productVariants, CancellationToken ct = default);
    Result<Guid> Create(Guid vendorId, string name, string? description);
    Result CreateBulk(Guid vendorId, IEnumerable<(string name, string? description)> values);
    Task<Result> DeleteAllProductVariantAsync(Guid productId, CancellationToken ct = default);
    Task<Result> DeleteProductVariantAsync(Guid id, CancellationToken ct = default);
    Task<Result> RemoveCategory(Guid productId, Guid categoryId, CancellationToken ct = default);
    Task<Result> UpdateDetails(Guid id, string name, string? description, CancellationToken ct = default);
    Task<Result> UpdateProductCategory(Guid productId, Guid categoryId, bool isPrimary, CancellationToken ct = default);
    Task<Result> UpdateProductStatus(Guid id, string status, CancellationToken ct = default);
    Task<Result> UpdateProductVariantCustomizationOptionsAsync(Guid id, ProductVariantsOption customOption, CancellationToken ct = default);
    Task<Result> UpdateProductVariantPriceAsync(Guid id, decimal price, decimal? compareAtPrice, string currency, CancellationToken ct = default);
}