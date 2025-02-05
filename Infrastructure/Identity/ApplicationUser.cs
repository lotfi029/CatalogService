using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public bool IsDisabled { get; set; }
    public string? VisitorType { get; set; }
    public string? Region { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
