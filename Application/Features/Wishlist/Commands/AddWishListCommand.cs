namespace Application.Features.Wishlist.Commands;
public record AddWishListCommand(string UserId, Guid ProductId) : IRequest<Result<Guid>>;
