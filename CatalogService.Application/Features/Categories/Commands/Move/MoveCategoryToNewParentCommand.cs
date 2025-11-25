using CatalogService.Domain.DomainService;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Errors;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Application.Features.Categories.Commands.Move;

public sealed record MoveCategoryToNewParentCommand(Guid Id, Guid NewParentId) : ICommand<IEnumerable<Category>>;

internal sealed class MoveCategoryToNewParentCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    ICategoryDomainService categoryDomainService,
    ILogger<MoveCategoryToNewParentCommandHandler> logger
    ) : ICommandHandler<MoveCategoryToNewParentCommand, IEnumerable<Category>>
{
    public async Task<Result<IEnumerable<Category>>> HandleAsync(MoveCategoryToNewParentCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty || command.NewParentId == Guid.Empty)
            return CategoryErrors.InvalidId;

        if (command.Id == command.NewParentId)
            return CategoryErrors.InvalidParentId;

        if (await categoryRepository.FindByIdAsync(command.Id, ct) is not { } category)
            return CategoryErrors.NotFound(command.Id);

        try
        {
            var result = await categoryRepository.GetCategoryTree(command.Id, ct); 
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occured while moving the category with id: {categoryId} to a new parent with id: {parentId}",
                command.Id,
                command.NewParentId);

            return Error.Unexpected($"Error occured while moving the category with id: {command.Id} to a new parent with id: {command.NewParentId}");
        }
    }
}

