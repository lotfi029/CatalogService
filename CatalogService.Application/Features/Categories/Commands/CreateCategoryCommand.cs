using CatalogService.Application.Abstractions;
using CatalogService.Domain.DomainService;
using Microsoft.Extensions.Logging;

namespace CatalogService.Application.Features.Categories.Commands;

public sealed record CreateCategoryCommand(
    string Name,
    string Slug,
    Guid? ParentId,
    string? Description,
    string? Path,
    Dictionary<string, object>? Metadata
    ) : ICommand<Guid>;


public sealed class CreateCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    CategoryDomainService categoryDomainService,
    ICategoryRepository repository,
    ILogger<CreateCategoryCommandHandler> logger) : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CreateCategoryCommand command, CancellationToken ct = default)
    {
        // when path is not null check if it valid and unique

        // generate a business logic for metadata if not null
        try
        {
            var category = await categoryDomainService.CreateCategoryAsync(
                name: command.Name,
                slug: command.Slug,
                parentId: command.ParentId,
                description: command.Description,
                ct: ct);

            await repository.AddAsync(category, ct);

            await unitOfWork.SaveChangesAsync(ct);
        
            return Result.Success(category.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating category");
            return Error.Unexpected("Error occurred while creating category");
        }
    }
}