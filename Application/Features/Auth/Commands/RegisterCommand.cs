using Application.Features.Auth.Contracts;

namespace Application.Features.Auth.Commands;
public record RegisterCommand(RegisterRequest Request) : IRequest<Result>;

