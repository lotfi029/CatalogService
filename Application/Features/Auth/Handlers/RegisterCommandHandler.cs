using Application.Features.Auth;
using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Handlers;

public class RegisterCommandHandler(IAuthService authService) : IRequestHandler<RegisterCommand, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = request.Request;

        var result = await _authService.RegisterAsync(user, cancellationToken);

        return result;
    }
}

