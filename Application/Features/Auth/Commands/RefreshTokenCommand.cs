namespace Application.Features.Auth.Commands;

public record RefreshTokenCommand(RefreshTokenRequest Request) : IRequest<Result<AuthenticationResponse>>;
