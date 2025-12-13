using CatalogService.Domain.DomainService.Categories;

namespace CatalogService.Application.Features.Categories.Commands.Delete;

public sealed record DeleteCategoryCommand(Guid Id, Guid? ParentId = null) : ICommand;

internal sealed class DeleteCategoryCommandHandler(
    ICategoryDomainService categoryService,
    IUnitOfWork unitOfWork,
    ILogger<DeleteCategoryCommandHandler> logger) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> HandleAsync(DeleteCategoryCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty && (command.ParentId is not null && command.ParentId == Guid.Empty))
            return CategoryErrors.InvalidId;


        using var transaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            if (await categoryService.DeleteAsync(command.Id, command.ParentId, ct) is { IsFailure: true } deleteError)
            {
                await unitOfWork.RollBackTransactionAsync(transaction, ct);
                return deleteError.Error;
            }

            await unitOfWork.SaveChangesAsync(ct);
            await unitOfWork.CommitTransactionAsync(transaction, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollBackTransactionAsync(transaction, ct);
            logger.LogError(ex,
                "Error deleting category with Id: {CategoryId}",
                command.Id);

            return CategoryErrors.DeleteCategory;
        }
    }
}