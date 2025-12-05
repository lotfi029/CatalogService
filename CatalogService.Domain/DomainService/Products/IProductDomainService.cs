


namespace CatalogService.Domain.DomainService.Products;

public interface IProductDomainService
{
    Result<Guid> Create(Guid vendorId, string name, string? description);
    Result CreateBulk(Guid vendorId, IEnumerable<(string name, string? description)> values);
    Task<Result> UpdateDetails(Guid id, string name, string? description, CancellationToken ct = default);
    Task<Result> UpdateProductStatus(Guid id, string status, CancellationToken ct = default);
}