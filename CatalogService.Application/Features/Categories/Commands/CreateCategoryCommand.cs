using CatalogService.Domain.Abstractions;
using CatalogService.Domain.DomainService;
using Microsoft.Extensions.Logging;

namespace CatalogService.Application.Features.Categories.Commands;

public sealed record CreateCategoryCommand(
    string Name,
    string Slug,
    Guid? ParentId,
    string? Description,
    Dictionary<string, object>? Metadata
    ) : ICommand<Guid>;


public sealed class CreateCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryDomainService categoryDomainService,
    ICategoryRepository repository,
    ILogger<CreateCategoryCommandHandler> logger) : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CreateCategoryCommand command, CancellationToken ct = default)
    {    
        try
        {
            var category = await categoryDomainService.CreateCategoryAsync(
                name: command.Name,
                slug: command.Slug,
                parentId: command.ParentId,
                description: command.Description,
                ct: ct);

            if (category.IsFailure)
                return category.Error;

            await repository.AddAsync(category.Value!, ct);
            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success(category.Value!.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating category");
            return Error.Unexpected("Error occurred while creating category");
        }
    }
}