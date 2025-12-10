using CatalogService.Application.DTOs.ProductAttributes;

namespace CatalogService.Application.Features.ProductAttributes.Queries;

public interface IProductAttributeQueries
{
    Task<IEnumerable<ProductAttributeResponse>> GetAllByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task<Result<ProductAttributeResponse>> GetAsync(Guid productId, Guid attributeId, CancellationToken ct = default);
}
