using Application.Features.Auth.Contracts;

namespace Infrastructure.Identity.Authentication;
public interface IJwtProvider
{
    BearerTokenResponse GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string? ValidateToken(string Token);

}
