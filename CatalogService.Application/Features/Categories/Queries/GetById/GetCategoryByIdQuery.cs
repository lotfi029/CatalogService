namespace CatalogService.Application.Features.Categories.Queries.GetById;

public sealed record GetCategoryByIdQuery(Guid id) : IQuery<Guid>;