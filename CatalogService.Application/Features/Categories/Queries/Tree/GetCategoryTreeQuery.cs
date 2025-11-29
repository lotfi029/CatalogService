using CatalogService.Application.DTOs.Categories;

namespace CatalogService.Application.Features.Categories.Queries.Tree;

public sealed record GetCategoryTreeQuery(Guid? ParentId) : IQuery<IEnumerable<CategoryResponse>>;