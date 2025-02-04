using Application.Features.Auth;
using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Handlers;

public class RegisterCommandHandler(IAuthService authService) : IRequestHandler<RegisterCommandRequest, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
    {
        var user = request.Request;

        var result = await _authService.RegisterAsync(user, cancellationToken);

        return result;
    }
}

