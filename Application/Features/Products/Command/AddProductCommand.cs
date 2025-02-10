namespace Application.Features.Products.Command;
public record AddProductCommand (ProductRequest Request) : IRequest<Result<Guid>>;
