using Application.Features.Wishlist.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.Wishlist.Handlers;

public class GetWishListProductQueryHandler(
    IProductRepository _productRepository,
    IWishListRepository _wishListRepository,
    IHttpContextAccessor _httpContextAccessor,
    LinkGenerator _linkGenerator
    ) 
    : IRequestHandler<GetWishListProductQuery, IEnumerable<ProductResponse>>
{
    public async Task<IEnumerable<ProductResponse>> Handle(GetWishListProductQuery request, CancellationToken cancellationToken)
    {
        var wishList = await _wishListRepository.GetAll(request.UserId, cancellationToken);
        
        if (!wishList.Any())
            return [];

        var products = await _productRepository.GetByIdsAsync(wishList, cancellationToken);

        var httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("invalid request");

        return products.Select(x => new ProductResponse
        (
            x.Id,
            x.Name,
            x.Description,
            _linkGenerator.GetUriByName(httpContext, "stream-image", new { x.ImageUrl })!,
            x.Quentity,
            x.Price,
            x.CategoryId
        ));

    }
}