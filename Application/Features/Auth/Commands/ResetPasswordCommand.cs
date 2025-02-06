namespace Application.Features.Auth.Commands;

public record ResetPasswordCommand(ResetPasswordRequest Request) : IRequest<Result>;