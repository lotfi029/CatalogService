using Application.Features.Products.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Net.Http;

namespace Application.Features.Products.Handlers;
public class GetProductByIdQueryHandler (
    IProductRepository _repository,
    IHttpContextAccessor _httpContextAccessor,
    LinkGenerator _linkGenerator
    ) : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        

        if (product.IsFailure)
            return Result.Failure<ProductResponse>(product.Error);

        var imageUrl = _linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, "stream-image", new { imageName = product.Value!.ImageUrl })!;

        var response = new ProductResponse(
            product.Value.Id, 
            product.Value.Name, 
            product.Value.Description, 
            imageUrl, 
            product.Value.Quentity, 
            product.Value.Price, 
            product.Value.CategoryId
        );

        return Result.Success(response);
    }
}
