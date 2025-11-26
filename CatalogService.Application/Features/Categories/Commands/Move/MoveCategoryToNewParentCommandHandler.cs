using CatalogService.Domain.DomainService;
using CatalogService.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace CatalogService.Application.Features.Categories.Commands.Move;

internal sealed class MoveCategoryToNewParentCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository repository,
    ICategoryDomainService categoryDomainService,
    ILogger<MoveCategoryToNewParentCommandHandler> logger
    ) : ICommandHandler<MoveCategoryToNewParentCommand>
{
    public async Task<Result> HandleAsync(MoveCategoryToNewParentCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty || command.NewParentId == Guid.Empty)
            return CategoryErrors.InvalidId;
        
        if (command.Id == command.NewParentId)
            return CategoryErrors.CannotMoveToSelf;

        if (await repository.FindByIdAsync(command.Id, ct) is not { } category)
            return CategoryErrors.NotFound(command.Id);

        if (category.ParentId == command.NewParentId)
            return CategoryErrors.AlreadyHasThisParent;

        if (await repository.FindByIdAsync(command.NewParentId, ct) is not { } parent)
            return CategoryErrors.ParentNotFound(command.NewParentId);

        using var transaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var result = await categoryDomainService.MoveToNewParent(category.Id, parent, ct);

            if (result.IsFailure)
            {
                logger.LogWarning(
                    "Domain service rejected move of category {CategoryId} to parent {ParentId}: {Error}",
                    command.Id, command.NewParentId, result.Error);
                return result;
            }

            var updatedCategories = result.Value!;

            repository.UpdateRange(updatedCategories);
            
            await unitOfWork.SaveChangesAsync(ct);
            await unitOfWork.CommitTransactionAsync(transaction, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);

            logger.LogError(ex,
                "Error occurred while moving category {CategoryId} to parent {ParentId}",
                command.Id, command.NewParentId);

            return Error.Unexpected("Failed to move category");
        }
    }
}

