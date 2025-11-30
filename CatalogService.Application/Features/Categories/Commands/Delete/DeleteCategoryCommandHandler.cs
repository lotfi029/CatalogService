namespace CatalogService.Application.Features.Categories.Commands.Delete;

internal sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    public Task<Result> HandleAsync(DeleteCategoryCommand command, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}