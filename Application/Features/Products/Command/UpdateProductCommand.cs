namespace Application.Features.Products.Command;

public record UpdateProductCommand(Guid Id, UpdateProductRequest Request)  : IRequest<Result>;
