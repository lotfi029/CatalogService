
namespace CatalogService.Application.Features.Categories.Commands.Delete;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand;
