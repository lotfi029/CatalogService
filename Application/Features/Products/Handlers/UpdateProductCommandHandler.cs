using Application.Features.Products.Command;
using Application.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace Application.Features.Products.Handlers;

public class UpdateProductCommandHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    IHubContext<ProductHub, IProductClient> _hubContext) : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IProductRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var updateProduct = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (updateProduct.IsFailure)
            return Result.Failure(updateProduct.Error);

        var product = updateProduct.Value!;

        product = command.Request.Adapt(product);

        var result = await _repository.UpdateAsync(product!, cancellationToken);

        if (result.IsFailure) return result;

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        await _hubContext.Clients.All.ProductUpdated(product.Adapt<ProductResponse>());

        return result;
    }
}
