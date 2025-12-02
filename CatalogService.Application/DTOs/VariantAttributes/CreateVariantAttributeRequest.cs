using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.VariantAttributes;

public sealed record CreateVariantAttributeRequest(
    string Code,
    string Name,
    string Datatype,
    bool AffectedInventory,
    AllowedValuesJson? AllowedValues
    );
