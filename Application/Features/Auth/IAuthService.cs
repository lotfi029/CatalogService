using Application.Features.Auth.Contracts;

namespace Application.Features.Auth;
public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result> ExpireTokenAsync();
}
