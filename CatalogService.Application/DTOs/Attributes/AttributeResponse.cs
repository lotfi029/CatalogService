namespace CatalogService.Application.DTOs.Attributes;

public sealed record AttributeResponse(
    string Name,
    string Code,
    bool IsFilterable,
    bool IsSearchable,
    string OptionsType);
