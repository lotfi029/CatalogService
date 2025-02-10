using Application.Features.Categories.Contracts;

namespace Application.Features.Categories.Queries;
public record GetCategoryByIdQuery(Guid Id) : IRequest<Result<CategoryResponse>>;
