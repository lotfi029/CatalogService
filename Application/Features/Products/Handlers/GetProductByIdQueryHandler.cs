using Application.Features.Products.Queries;

namespace Application.Features.Products.Handlers;
public class GetProductByIdQueryHandler (
    IProductRepository repository) : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    private readonly IProductRepository _repository = repository;

    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetProductByIdAsync(request.Id, cancellationToken);

        if (product.IsFailure)
            return Result.Failure<ProductResponse>(product.Error);

        var response = product.Value.Adapt<ProductResponse>();

        return Result.Success(response);
    }
}
