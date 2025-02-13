namespace Application.Errors;

public class WishListErrors
{
    public static readonly Error ProductNotFound
        = Error.NotFound(nameof(ProductNotFound), "product not found");

    public static readonly Error ProductAlreadyInWishList
        = Error.Conflict(nameof(ProductAlreadyInWishList), "product already in wish list");
}