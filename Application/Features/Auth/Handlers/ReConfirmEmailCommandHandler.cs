using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Handlers;

public class ReConfirmEmailCommandHandler(IAuthService service) : IRequestHandler<ReConfirmEmailCommand, Result>
{
    private readonly IAuthService _service = service;

    public async Task<Result> Handle(ReConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var result = await _service.ReConfirmEmailAsync(request);

        return result;
    }
}
