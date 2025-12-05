using CatalogService.Application.DTOs.Products;

namespace CatalogService.Application.Features.Products.Queries;

public interface IProductQueries
{
    Task<Result<IEnumerable<ProductResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<ProductDetailedResponse>> GetAsync(Guid id, CancellationToken ct = default);
}
