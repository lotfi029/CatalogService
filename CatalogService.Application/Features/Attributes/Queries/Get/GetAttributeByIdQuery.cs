using CatalogService.Application.DTOs.Attributes;

namespace CatalogService.Application.Features.Attributes.Queries.Get;

public sealed record GetAttributeByIdQuery() : IQuery<AttirbuteDetailedResponse>;