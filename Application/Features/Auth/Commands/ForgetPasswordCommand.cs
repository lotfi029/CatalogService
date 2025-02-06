namespace Application.Features.Auth.Commands;

public record ForgetPasswordCommand(ForgetPasswordRequest Request) : IRequest<Result>;