using CatalogService.Domain.DomainService.Categories;

namespace CatalogService.Application.Features.Categories.Commands.Create;

internal sealed class CreateCategoryCommandHandler(
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
                isActive: command.IsActive,
                parentId: command.ParentId,
                description: command.Description,
                ct: ct);

            if (category.IsFailure)
                return category.Error;

            repository.Add(category.Value!);
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