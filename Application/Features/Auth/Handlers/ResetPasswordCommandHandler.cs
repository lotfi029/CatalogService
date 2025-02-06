using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Handlers;

public class ResetPasswordCommandHandler(IAuthService service) : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IAuthService _service = service;

    public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var result = await _service.ResetPasswordAsync(request);

        return result;
    }
}
