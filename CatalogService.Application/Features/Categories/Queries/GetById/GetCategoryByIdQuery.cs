using CatalogService.Application.DTOs.Categories;

namespace CatalogService.Application.Features.Categories.Queries.GetById;

public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryDetailedResponse>;

