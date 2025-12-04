using CatalogService.Application.DTOs.Attributes;

namespace CatalogService.Application.Features.Attributes.Queries.GetByType;

public sealed record GetAttributeByTypeQuery(Guid Id) : IQuery<IEnumerable<AttributeResponse>>;
