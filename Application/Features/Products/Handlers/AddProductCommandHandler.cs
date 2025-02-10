using Application.Features.Products.Command;

namespace Application.Features.Products.Handlers;
public class AddProductCommandHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddProductCommand, Result<Guid>>
{
    private readonly IProductRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(AddProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Request.Name,
            Description = command.Request.Description,
            Price = command.Request.Price,
            Quentity = command.Request.Quentity,
            CategoryId = command.Request.CategoryId,
        };

        var result = await _repository.AddProductAsync(product, cancellationToken);

        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return Result.Success(result.Value);
    }
}
