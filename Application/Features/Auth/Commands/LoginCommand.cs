namespace Application.Features.Auth.Commands;
public record LoginCommand(LoginRequest Request) : IRequest<Result<AuthenticationResponse>>;
