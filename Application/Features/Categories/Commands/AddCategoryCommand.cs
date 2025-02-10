using Application.Features.Categories.Contracts;

namespace Application.Features.Categories.Commands;
public record AddCategoryCommand(CategoryRequest Request) : IRequest<Result<Guid>>;


