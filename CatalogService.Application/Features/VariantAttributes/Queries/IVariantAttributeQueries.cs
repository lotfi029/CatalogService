
using CatalogService.Application.DTOs.VariantAttributes;

namespace CatalogService.Application.Features.VariantAttributes.Queries;

public interface IVariantAttributeQueries
{
    Task<Result<IEnumerable<VariantAttributeResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<VariantAttributeResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
}
