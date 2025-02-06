namespace Application.Features.Auth;
public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthenticationResponse>> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthenticationResponse>> GetRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result> ReConfirmEmailAsync(ResendConfirmEmailRequest request); 
    Task<Result> RevokeAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result> SendResetPasswordTokenAsync(ForgetPasswordRequest email);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
    Task<Result> ExpireTokenAsync();
}
