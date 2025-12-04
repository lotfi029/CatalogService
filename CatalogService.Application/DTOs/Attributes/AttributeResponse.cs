namespace CatalogService.Application.DTOs.Attributes;

public sealed record AttributeResponse(
    Guid Id,
    string Name,
    string Code,
    bool IsFilterable,
    bool IsSearchable,
    string OptionsType)
{
    private AttributeResponse() : this(
        Id: Guid.Empty,
        Name: string.Empty,
        Code: string.Empty,
        IsFilterable: true,
        IsSearchable: true,
        OptionsType: string.Empty) { }
};
