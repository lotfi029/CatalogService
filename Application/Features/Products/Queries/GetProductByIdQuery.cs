namespace Application.Features.Products.Queries;
public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductResponse>>;