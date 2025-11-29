using CatalogService.Application.DTOs.Categories;

namespace CatalogService.Application.Features.Categories.Queries.GetBySlug;

public sealed record GetCategoryBySlugQuery(string Slug) : IQuery<CategoryDetailedResponse>;
