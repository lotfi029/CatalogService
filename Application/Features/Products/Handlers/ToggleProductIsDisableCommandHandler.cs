using Application.Features.Products.Command;

namespace Application.Features.Products.Handlers;

public class ToggleProductIsDisableCommandHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<ToggleProductIsDisableCommand , Result>
{
    private readonly IProductRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(ToggleProductIsDisableCommand command, CancellationToken cancellationToken)
    {
        var productResult = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (productResult.IsFailure)
            return Result.Failure(productResult.Error);

        var product = productResult.Value!;

        product.IsDisabled = !product.IsDisabled;

        var updateResult = await _repository.UpdateAsync(product, cancellationToken);

        if (updateResult.IsFailure)
            return updateResult;

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return updateResult;
    }
}
