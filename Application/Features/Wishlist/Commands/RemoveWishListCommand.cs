namespace Application.Features.Wishlist.Commands;

public record RemoveWishListCommand(string UserId, Guid ProductId) : IRequest<Result>;
