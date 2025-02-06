using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Handlers;

public sealed class RefreshTokenCommandHandler(IAuthService authService) : IRequestHandler<RefreshTokenCommand, Result<AuthenticationResponse>>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<AuthenticationResponse>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var result = await _authService.GetRefreshTokenAsync(request, cancellationToken);

        return result;
    }
}
