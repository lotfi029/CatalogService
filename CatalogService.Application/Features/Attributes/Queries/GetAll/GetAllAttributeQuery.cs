using CatalogService.Application.DTOs.Attributes;

namespace CatalogService.Application.Features.Attributes.Queries.GetAll;

public sealed record GetAllAttributeQuery(string OptionType) : IQuery<IEnumerable<AttributeResponse>>;