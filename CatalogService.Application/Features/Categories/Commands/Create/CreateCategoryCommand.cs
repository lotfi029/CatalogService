namespace CatalogService.Application.Features.Categories.Commands.Create;

public sealed record CreateCategoryCommand(
    string Name,
    string Slug,
    bool IsActive,
    Guid? ParentId,
    string? Description
    ) : ICommand<Guid>;
