namespace Application.Features.Products.Queries;

public record GetProductInCategoryQuery(Guid CategoryId) : IRequest<IEnumerable<ProductResponse>>;
