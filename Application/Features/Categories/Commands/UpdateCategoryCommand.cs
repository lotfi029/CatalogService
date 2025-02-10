using Application.Features.Categories.Contracts;

namespace Application.Features.Categories.Commands;

public record UpdateCategoryCommand(Guid Id, CategoryRequest Request) : IRequest<Result>;


