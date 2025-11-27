
namespace CatalogService.Application.Features.Categories.Commands.Delete;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand;

public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    public Task<Result> HandleAsync(DeleteCategoryCommand command, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}