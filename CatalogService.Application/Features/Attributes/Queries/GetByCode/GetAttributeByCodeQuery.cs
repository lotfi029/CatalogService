using CatalogService.Application.DTOs.Attributes;

namespace CatalogService.Application.Features.Attributes.Queries.GetByCode;

public sealed record GetAttributeByCodeQuery(string Code) : IQuery<AttirbuteDetailedResponse>;