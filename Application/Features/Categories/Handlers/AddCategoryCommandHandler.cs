using Application.Features.Categories.Commands;

namespace Application.Features.Categories.Handlers;
public sealed class AddCategoryCommandHandler(
    ICategoryRepository _categoryRepository,
    IUnitOfWork _unitOfWork) : IRequestHandler<AddCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = command.Request.Adapt<Category>();

        await _categoryRepository.AddAsync(category, cancellationToken);

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return Result.Success(category.Id);
    }
}
