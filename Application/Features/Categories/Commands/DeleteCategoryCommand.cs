namespace Application.Features.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : IRequest<Result>;


