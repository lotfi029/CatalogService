using Application.Features.Orders.Contracts;

namespace Application.Features.Orders.Command;
public record CreateOrderCommand(string UserId, Guid ProductId, CreateOrderRequest Request) : IRequest<Result<Guid>>;