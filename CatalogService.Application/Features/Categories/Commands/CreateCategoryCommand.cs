using CatalogService.Application.Abstractions;
using CatalogService.Domain.Entities;
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
    ILogger<CreateCategoryCommandHandler> logger) : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CreateCategoryCommand command, CancellationToken ct = default)
    {
        // if parent id not null, check if parent category exists
        // then check if level is valid
        
        // check for uniqueness of slug
        // when path is not null check if it valid and unique

        // generate a business logic for metadata if not null
        try
        {
            int level = 0;
            if (command.ParentId is not null && command.ParentId != Guid.Empty)
            {
                var parents = await unitOfWork.Categories.GetAllParentAsync(command.ParentId.Value, ct);
                level = (parents is not null && parents.Any()) ? parents.Count() : 0;
            }
        
            var category = Category.Create(
                command.Name,
                command.Slug.ToLower(),
                (short)level,
                command.ParentId,
                command.Description,
                command.Path,
                command.Metadata
                );

            await unitOfWork.Categories.AddAsync(category, ct);
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