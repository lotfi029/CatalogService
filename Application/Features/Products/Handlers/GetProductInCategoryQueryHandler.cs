using Application.Features.Products.Queries;

namespace Application.Features.Products.Handlers;

public class GetProductInCategoryQueryHandler(
    IProductRepository repository) : IRequestHandler<GetProductInCategoryQuery, IEnumerable<ProductResponse>>
{
    private readonly IProductRepository _repository = repository;

    public async Task<IEnumerable<ProductResponse>> Handle(GetProductInCategoryQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAllProductAsync(cancellationToken: cancellationToken);

        var categoryProducts = product.Where(e => e.CategoryId == request.CategoryId);

        var response = categoryProducts.Adapt<IEnumerable<ProductResponse>>();

        return response;
    }
}
