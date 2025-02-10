namespace Application.Features.Products.Command;

public record DeleteProductCommand(Guid Id) : IRequest<Result>;