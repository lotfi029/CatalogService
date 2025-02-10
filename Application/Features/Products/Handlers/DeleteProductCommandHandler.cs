using Application.Features.Products.Command;
using Mapster;


namespace Application.Features.Products.Handlers;

public class DeleteProductCommandHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IProductRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var productResult = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (productResult.IsFailure)
            return Result.Failure(productResult.Error);

        var product = productResult.Value!;

        product.IsDeleted = true;

        var updateResult = await _repository.UpdateAsync(product, cancellationToken);

        if (updateResult.IsFailure)
            return updateResult;

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return updateResult;
    }
}
