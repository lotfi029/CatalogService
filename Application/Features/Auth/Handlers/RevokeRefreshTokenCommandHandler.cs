using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Handlers;

public class RevokeRefreshTokenCommandHandler(IAuthService service) : IRequestHandler<RevokeRefreshTokenCommand, Result>
{
    private readonly IAuthService _service = service;

    public async Task<Result> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var result = await _service.RevokeAsync(request, cancellationToken);

        return result;
    }
}
