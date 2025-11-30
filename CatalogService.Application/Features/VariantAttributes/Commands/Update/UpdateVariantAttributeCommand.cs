using CatalogService.Application.DTOs.VariantAttributes;

namespace CatalogService.Application.Features.VariantAttributes.Commands.Update;

public sealed record UpdateVariantAttributeCommand(Guid Id, UpdateVariantAttributeRequest Request) : ICommand;