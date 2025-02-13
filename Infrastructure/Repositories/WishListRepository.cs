namespace Infrastructure.Repositories;
public class WishListRepository(ApplicationDbContext _conetxt) : IWishListRepository
{
    public async Task<Result<Guid>> AddAsync(WishList wishList, CancellationToken cancellationToken = default)
    {
        if (wishList is null)
            return Result.Failure<Guid>(WishListErrors.ProductNotFound);

        if (!await _conetxt.Users.AnyAsync(e => e.Id == wishList.UserId, cancellationToken))
            return Result.Failure<Guid>(AuthenticationErrors.UnAutherizationAccess);

        if (await _conetxt.WishList.AnyAsync(e => e.ProductId == wishList.ProductId && e.UserId == wishList.UserId, cancellationToken))
            return Result.Failure<Guid>(WishListErrors.ProductAlreadyInWishList);

        await _conetxt.WishList.AddAsync(wishList, cancellationToken);
        
        await _conetxt.SaveChangesAsync(cancellationToken);

        return Result.Success(wishList.Id);
    }

    public async Task<Result> RemoveAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        var rowDeleted = await _conetxt.WishList
            .Where(x => x.ProductId == id && x.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);

        return rowDeleted == 0 
            ? WishListErrors.ProductNotFound 
            : Result.Success();
    }
    public async Task<IEnumerable<Guid>> GetAll(string userId, CancellationToken cancellationToken = default)
    {
        var wishList = await _conetxt.WishList
            .Where(x => x.UserId == userId)
            .Select(x => x.ProductId)
            .ToListAsync(cancellationToken);

        if (wishList is null)
            return [];

        return wishList;
    }
}
