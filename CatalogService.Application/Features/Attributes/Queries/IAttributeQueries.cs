using CatalogService.Application.DTOs.Attributes;
using CatalogService.Domain.Enums;

namespace CatalogService.Application.Features.Attributes.Queries;

public interface IAttributeQueries
{
    Task<Result<IEnumerable<AttributeResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<AttributeDetailedResponse>> GetAsync(Guid id, CancellationToken ct = default);
    Task<Result<AttributeDetailedResponse>> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<Result<IEnumerable<AttributeResponse>>> GetByOptionsTypeAsync(string type, CancellationToken ct = default);
}
