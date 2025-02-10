namespace Application.Features.Products.Queries;
public record GetAllProductsQuery : IRequest<IEnumerable<ProductResponse>>;
