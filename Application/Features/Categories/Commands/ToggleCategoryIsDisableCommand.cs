namespace Application.Features.Categories.Commands;

public record ToggleCategoryIsDisableCommand(Guid Id) : IRequest<Result>;


