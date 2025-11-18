using CatalogService.Application.Abstractions;

namespace CatalogService.Application.Features.Categories.Commands;

public sealed record CreateCategoryCommand(
    string Name,
    string Slug,
    string Level,
    Guid? ParentId,
    string? Description,
    string? Path,
    Dictionary<string, object>? Metadata
    ) : ICommand<Guid>;


public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    public Task<Result<Guid>> HandleAsync(CreateCategoryCommand command, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}