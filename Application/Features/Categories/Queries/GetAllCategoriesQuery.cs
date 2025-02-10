using Application.Features.Categories.Contracts;

namespace Application.Features.Categories.Queries;

public record GetAllCategoriesQuery() : IRequest<IEnumerable<CategoryResponse>>;
