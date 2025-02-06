namespace Application.Features.Auth.Commands;
public record ConfirmEmailCommand(ConfirmEmailRequest Request) : IRequest<Result>;
