namespace CatalogService.Application.DTOs.ProductCategories;

public sealed record ProductCategoryResponse(
    Guid CategoryId,
    string CategoryName,
    string CategorySlug,
    bool IsPrimary)
{
    private ProductCategoryResponse() : this(
        CategoryId: Guid.Empty,
        CategoryName: string.Empty,
        CategorySlug: string.Empty,
        IsPrimary: false)
    { }
}