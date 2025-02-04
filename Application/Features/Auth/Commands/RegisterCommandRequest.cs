using Application.Features.Auth.Contracts;

namespace Application.Features.Auth.Commands;
public record RegisterCommandRequest(RegisterRequest Request) : IRequest<Result>;

