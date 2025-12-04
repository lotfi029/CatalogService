using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.Attributes;

public sealed record AttirbuteDetailedResponse(
    string Name,
    string Code,
    bool IsFilterable,
    bool IsSearchable,
    string OptionsType,
    ValuesJson? Options,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt);