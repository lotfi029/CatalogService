using Application.Features.Products.Command;
using Mapster;


namespace Application.Features.Products.Handlers;

public class UpdateProductCommandHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IProductRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var updateProduct = await _repository.GetProductByIdAsync(command.Id, cancellationToken);

        if (updateProduct.IsFailure)
            return Result.Failure(updateProduct.Error);

        var product = updateProduct.Value.Adapt<Product>();

        product = command.Request.Adapt(product);

        var result = await _repository.UpdateProductAsync(product, cancellationToken);

        if (result.IsFailure) return result;

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return result;
    }
}
