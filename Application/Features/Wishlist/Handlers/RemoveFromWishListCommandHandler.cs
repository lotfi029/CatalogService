using Application.Features.Wishlist.Commands;

namespace Application.Features.Wishlist.Handlers;

public class RemoveFromWishListCommandHandler(
    IWishListRepository _wishListRepository)
    : IRequestHandler<RemoveWishListCommand, Result>
{
    public async Task<Result> Handle(RemoveWishListCommand request, CancellationToken cancellationToken)
    {
        var result = await _wishListRepository.RemoveAsync(request.UserId, request.ProductId, cancellationToken);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        return Result.Success();
    }
}