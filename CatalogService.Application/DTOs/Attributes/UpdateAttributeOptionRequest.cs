using CatalogService.Domain.Enums;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.Attributes;

public sealed record UpdateAttributeOptionRequest(ValuesJson Option);
