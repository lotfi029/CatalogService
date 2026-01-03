namespace CatalogService.Domain.Errors;

public class ProductCategoriesErrors
{
    private const string _code = "productCategories";
    public static Error InvalidId
        => Error.BadRequest(
       $"{_code}.{nameof(InvalidId)}",
       $"Please enter a valid id");
    public static Error NotFound
        => Error.NotFound(
            $"{_code}.{NotFound}",
            "product category not found");

    public static Error DuplicatedCategory(Guid categoryId)
        => Error.Conflict(
            $"{_code}.{DuplicatedCategory}",
            $"category with id: '{categoryId}' is exist already");

    public static Error InvalidIncludedVariants(IList<string> variants)
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidIncludedVariants)}",
            $"You must at least include this variants {string.Join(", ", variants)} with your request");

    public static Error InvalidActivation =>
        Error.BadRequest(
            $"{_code}.{nameof(InvalidActivation)}",
            "There is one or more required variant the related with this category must be definit first");

    public static Error AddProductCategory =>
        Error.Unexpected("Failed to add product category");
    public static Error UpdateProductCategory =>
        Error.Unexpected("Failed to update product category");
    public static Error ActiveProductCategory =>
        Error.Unexpected("Failed to activate product category");
    public static Error DeleteProductCategory =>
        Error.Unexpected("Failed to delete product category");
    public static Error GetProductCategory =>
        Error.Unexpected("Failed to retrieve product category");
}
