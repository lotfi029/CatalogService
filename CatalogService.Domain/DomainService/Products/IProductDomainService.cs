


namespace CatalogService.Domain.DomainService.Products;

public interface IProductDomainService
{
    Task<Result> AddProductCategory(Guid productId, Guid categoryId, bool isPrimary, CancellationToken ct = default);
    Result<Guid> Create(Guid vendorId, string name, string? description);
    Result CreateBulk(Guid vendorId, IEnumerable<(string name, string? description)> values);
    Task<Result> RemoveCategory(Guid productId, Guid categoryId, CancellationToken ct = default);
    Task<Result> UpdateDetails(Guid id, string name, string? description, CancellationToken ct = default);
    Task<Result> UpdateProductCategory(Guid productId, Guid categoryId, bool isPrimary, CancellationToken ct = default);
    Task<Result> UpdateProductStatus(Guid id, string status, CancellationToken ct = default);
}