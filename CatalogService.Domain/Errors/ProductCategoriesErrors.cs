namespace CatalogService.Domain.Errors;

public class ProductCategoriesErrors
{
    private const string _code = "productCategories";
    public static Error NotFound
        => Error.NotFound(
            $"{_code}.{NotFound}",
            "product category not found");

    public static Error DuplicatedCategory(Guid categoryId)
        => Error.Conflict(
            $"{_code}.{DuplicatedCategory}",
            $"category with id: '{categoryId}' is exist already");
}