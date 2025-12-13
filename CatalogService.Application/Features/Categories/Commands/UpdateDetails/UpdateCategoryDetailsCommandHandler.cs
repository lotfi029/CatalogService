namespace CatalogService.Application.Features.Categories.Commands.UpdateDetails;

internal sealed class UpdateCategoryDetailsCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    ILogger<UpdateCategoryDetailsCommandHandler> logger
    ) : ICommandHandler<UpdateCategoryDetailsCommand>
{
    public async Task<Result> HandleAsync(UpdateCategoryDetailsCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return CategoryErrors.InvalidId;

        if (await categoryRepository.FindAsync(command.Id, null, ct) is not { } category)
            return CategoryErrors.NotFound(command.Id);

        try
        {
            category.UpdateDetails(command.Request.Name, command.Request.Description);
            categoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while updating category details for category {CategoryId}",
                command.Id);
            return Error.Unexpected($"Error Occured While Update Category Details With Id: {command.Id}");
        }
    }
}