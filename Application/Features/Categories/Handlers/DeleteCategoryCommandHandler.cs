using Application.Features.Categories.Commands;

namespace Application.Features.Categories.Handlers;

public sealed class DeleteCategoryCommandHandler(
    ICategoryRepository _categoryRepository,
    IUnitOfWork _unitOfWork) : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id, cancellationToken);
        if (category.IsFailure)
            return category.Error;

        category.Value!.IsDisabled = true;

        var result = await _categoryRepository.UpdateAsync(category.Value, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return Result.Success();
    }
}
