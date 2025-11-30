namespace CatalogService.Application.Features.VariantAttributes.Commands.Delete;

public sealed record DeleteVariantAttributeCommand(Guid Id) : ICommand;