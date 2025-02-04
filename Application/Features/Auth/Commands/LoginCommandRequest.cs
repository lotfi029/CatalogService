using Application.Features.Auth.Contracts;

namespace Application.Features.Auth.Commands;

public record LoginCommandRequest(LoginRequest Request) : IRequest<Result>;
