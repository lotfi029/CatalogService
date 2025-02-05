namespace Application.Features.Auth;
public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthenticationResponse>> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result> ExpireTokenAsync();
}
