namespace Application.IRepositories;
public interface IWishListRepository
{
    Task<Result<Guid>> AddAsync(WishList wishList, CancellationToken cancellationToken = default);
    Task<Result> RemoveAsync(string userId, Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Guid>> GetAll(string userId, CancellationToken cancellationToken = default);
}
