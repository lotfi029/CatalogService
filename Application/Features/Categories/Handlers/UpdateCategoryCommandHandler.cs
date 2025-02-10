using Application.Features.Categories.Commands;

namespace Application.Features.Categories.Handlers;

public sealed class UpdateCategoryCommandHandler(
    ICategoryRepository _categoryRepository,
    IUnitOfWork _unitOfWork) : IRequestHandler<UpdateCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id, cancellationToken);
        
        if (category.IsFailure)
            return category.Error;

        var updatedCategory = category.Value!;

        updatedCategory = command.Request.Adapt(updatedCategory);

        var result = await _categoryRepository.UpdateAsync(updatedCategory, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return Result.Success();
    }
}
