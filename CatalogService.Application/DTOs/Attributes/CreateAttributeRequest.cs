using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.Attributes;

public sealed record CreateAttributeRequest(
    string Name,
    string Code,
    string OptionsType,
    bool IsFilterable,
    bool IsSearchable,
    ValuesJson Options);