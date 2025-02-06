using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Handlers;

public class ForgetPasswordCommandHandler(IAuthService service) : IRequestHandler<ForgetPasswordCommand, Result>
{
    private readonly IAuthService _service = service;

    public async Task<Result> Handle(ForgetPasswordCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var result = await _service.SendResetPasswordTokenAsync(request);

        return result;
    }
}
