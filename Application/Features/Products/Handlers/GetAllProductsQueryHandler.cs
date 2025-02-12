using Application.Features.Products.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.Products.Handlers;

public class GetAllProductsQueryHandler(
    IProductRepository _repository,
    LinkGenerator _linkGenerator,
    IHttpContextAccessor _httpContextAccessor) : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{

    public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAllAsync(cancellationToken:  cancellationToken);

        if (product is null)
            return [];


        var httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("now http context avaliable!");

        var response = product.Select(p => new ProductResponse(
            p.Id,
            p.Name,
            p.Description,
            _linkGenerator.GetUriByName(httpContext, "stream-image", new { imageName = p.ImageUrl })!,
            p.Quentity,
            p.Price,
            p.CategoryId
            ));

        return response;
    }
}
