namespace CatalogService.Application.Features.Categories.Commands.Move;

public sealed record MoveCategoryToNewParentCommand(Guid Id, Guid NewParentId) : ICommand;