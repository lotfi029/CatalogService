using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Handlers;
public class ConfirmEmailCommandHandler(IAuthService service) : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly IAuthService _service = service;

    public async Task<Result> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var result = await _service.ConfirmEmailAsync(request);

        return result;
    }
}
