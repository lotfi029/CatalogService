namespace Application.Features.Auth.Commands;

public record RevokeRefreshTokenCommand(RefreshTokenRequest Request) : IRequest<Result>;