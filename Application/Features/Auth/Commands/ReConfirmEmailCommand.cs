namespace Application.Features.Auth.Commands;

public record ReConfirmEmailCommand(ResendConfirmEmailRequest Request) : IRequest<Result>;
