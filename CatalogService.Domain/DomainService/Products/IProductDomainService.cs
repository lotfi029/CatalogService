namespace CatalogService.Domain.DomainService.Products;

public interface IProductDomainService
{
    Result<Guid> Create(Guid vendorId, string name, string? description);
    Result CreateBulk(Guid vendorId, IEnumerable<(string name, string? description)> values);
    Task<Result> UpdateDetails(Guid userId, Guid id, string name, string? description, CancellationToken ct = default);
    Task<Result> ActivateAsync(Guid userId, Guid productId, CancellationToken ct = default);
    Task<Result> ArchiveAsync(Guid userId, Guid productId, CancellationToken ct = default);

    Task<Result> AddProductCategory(
        Guid userId,
        Guid productId, 
        Guid categoryId, 
        bool isPrimary,
        CancellationToken ct = default);
    Task<Result> UpdateProductCategoryAsync(Guid userId, Guid productId, Guid categoryId, bool isPrimary, CancellationToken ct = default);
    Task<Result> DeleteCategoryAsync(Guid userId, Guid productId, Guid categoryId, CancellationToken ct = default);

    Task<Result> AddAttributeBulkAsync(Guid userId, Guid productId, IEnumerable<(Guid attributeId, string value)> values, CancellationToken ct = default);
    Task<Result> AddAttributeAsync(Guid userId, Guid productId, Guid attributeId, string value, CancellationToken ct = default);
    Task<Result> UpdateAttributeValueAsync(Guid userId, Guid productId, Guid attributeId, string newValue, CancellationToken ct = default);
    Task<Result> DeleteAttributeAsync(Guid userId, Guid productId, Guid attributeId, CancellationToken ct = default);
    Task<Result> DeleteAllAttributeAsync(Guid userId, Guid productId, CancellationToken ct = default);
    
    Task<Result> AddProductVariantAsync(
        Guid userId, 
        Guid productId, 
        decimal price, 
        decimal? compareAtPrice, 
        Dictionary<Guid, string> variantAttributes, 
        CancellationToken ct = default);
    Task<Result> UpdateProductVariantPriceAsync(
        Guid userId, 
        Guid id, 
        decimal price, 
        decimal? compareAtPrice, 
        string currency, 
        CancellationToken ct = default);
    Task<Result> DeleteProductVariantAsync(
        Guid userId, 
        Guid productId, 
        Guid variantId, 
        CancellationToken ct = default);
}