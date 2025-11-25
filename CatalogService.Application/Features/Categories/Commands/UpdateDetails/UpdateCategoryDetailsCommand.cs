using CatalogService.Application.DTOs.Categories;

namespace CatalogService.Application.Features.Categories.Commands.UpdateDetails;

public sealed record UpdateCategoryDetailsCommand(Guid Id, UpdateCategoryDetailsRequest Request) : ICommand;
