namespace CatalogService.Application.DTOs.Attributes;

public sealed record UpdateAttributeDetailsRequest(
    string Name,
    bool IsFilterable,
    bool IsSearchable);
