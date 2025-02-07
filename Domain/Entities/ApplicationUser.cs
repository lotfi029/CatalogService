using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public bool IsDisabled { get; set; }
    public string? VisitorType { get; set; }
    public string? Region { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<Favourite> Favourites { get; set; } = [];
    public ICollection<BuyingHistory> BuyingHistories { get; set; } = [];
    public ICollection<WishList> WishLists { get; set; } = [];
}
