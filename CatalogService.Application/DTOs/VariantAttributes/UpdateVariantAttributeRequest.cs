using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.VariantAttributes;

public sealed record UpdateVariantAttributeRequest(
    string Name,
    AllowedValuesJson? AllowedValues
    );
