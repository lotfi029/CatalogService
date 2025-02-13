using Application.Features.Wishlist.Commands;

namespace Application.Features.Wishlist.Handlers;
public class AddToWishListCommandHandler(
    IWishListRepository _wishListRepository,
    IProductRepository _productRepository)
    : IRequestHandler<AddWishListCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddWishListCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product.IsFailure)
            return Result.Failure<Guid>(product.Error);

        var wishList = new WishList
        {
            ProductId = request.ProductId,
            UserId = request.UserId
        };

        var result = await _wishListRepository.AddAsync(wishList, cancellationToken);

        if (result.IsFailure)
            return Result.Failure<Guid>(result.Error);
        
        return Result.Success(result.Value);
    }
}
