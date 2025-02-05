namespace Infrastructure.Identity;
public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresOn { get; set; }
    public DateTime CreateOn { get; set; }
    public DateTime? RevokeOn { get; set; }

    public bool IsExpires => ExpiresOn <= DateTime.UtcNow;
    public bool IsActive => RevokeOn is null && !IsExpires;

}

