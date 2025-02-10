using Application.Features.Products.Queries;

namespace Application.Features.Products.Handlers;

public class GetAllProductsQueryHandler(
    IProductRepository repository) : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{
    private readonly IProductRepository _repository = repository;

    public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAllAsync(cancellationToken:  cancellationToken);

        var response = product.Adapt<IEnumerable<ProductResponse>>();

        return response;
    }
}
