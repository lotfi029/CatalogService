using Application.Features.Auth;
using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Handlers;

public sealed class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommandRequest, Result>
{
    private readonly IAuthService _authService = authService;
    public async Task<Result> Handle(LoginCommandRequest command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var result = await _authService.GetTokenAsync(request, cancellationToken);

        return result;
    }
}
