namespace Application.Features.Buying.Command;
public record BuyProductCommand(string UserId, Guid ProductId) : IRequest<Result>;