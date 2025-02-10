namespace Application.Features.Products.Command;

public record ToggleProductIsDisableCommand(Guid Id) : IRequest<Result>;
