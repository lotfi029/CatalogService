using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.VariantAttributes;

public sealed record VariantAttributeResponse(
    Guid Id,
    string Code,
    string Name,
    string Datatype,
    AllowedValuesJson? AllowedValues,
    bool AffectsInventory
    );