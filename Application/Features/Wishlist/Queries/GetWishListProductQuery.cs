namespace Application.Features.Wishlist.Queries;
public record GetWishListProductQuery(string UserId) : IRequest<IEnumerable<ProductResponse>>;
