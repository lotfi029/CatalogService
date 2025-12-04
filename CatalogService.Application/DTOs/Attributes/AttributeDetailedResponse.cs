using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.Attributes;

public sealed record AttributeDetailedResponse(
    Guid Id,
    string Name,
    string Code,
    bool IsFilterable,
    bool IsSearchable,
    string OptionsType,
    ValuesJson? Options,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    private AttributeDetailedResponse() : this(
        Id: Guid.Empty,
        Name: string.Empty,
        Code: string.Empty,
        IsFilterable: true,
        IsSearchable: true,
        OptionsType: string.Empty,
        Options: null,
        IsActive: true,
        CreatedAt: default,
        UpdatedAt: null) { }
};