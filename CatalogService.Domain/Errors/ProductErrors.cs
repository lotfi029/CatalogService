namespace CatalogService.Domain.Errors;

public class ProductErrors
{
    private const string _code = "products";
    public static Error InvalidId
    => Error.BadRequest(
        $"{_code}.{nameof(InvalidId)}",
        $"Please enter a valid id");
    public static Error InvalidProductStatus(string status)
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidProductStatus)}",
            $"this status '{status}' invalid for product");

    public static Error NotFound(Guid id)
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}",
            $"product with id: '{id}' not found");

    public static Error CategoriesNotFound
        => Error.BadRequest(
            $"{_code}.{nameof(CategoriesNotFound)}",
            "you must assign the product to at least one category");
    public static Error ProductAlreadyActive
        => Error.Conflict(
            $"{_code}.{nameof(ProductAlreadyActive)}",
            "this product is already active");
    public static Error InvlalidActivateProcess
        => Error.Conflict(
            $"{_code}.{nameof(InvlalidActivateProcess)}",
            "to active the product you must be add at least one category, one variant, and one attribute");

    public static Error ProductAlreadyArchived
        => Error.Conflict(
            $"{_code}.{nameof(ProductAlreadyArchived)}",
            "this product is archived and cannot be active");

    public static Error ProductIsArchived
        => Error.Conflict(
            $"{_code}.{nameof(ProductIsArchived)}",
            "this product is archived and cannot be do any operation on it");
    public static Error CreateProduct
        => Error.Unexpected("Failed to create product");
    public static Error CreateBulkProduct
        => Error.Unexpected("Failed to create bulk product");
    public static Error UpdateProductDetails
        => Error.Unexpected("Failed to update details of the product");
    public static Error UpdateProductStatus
        => Error.Unexpected("Failed to update status of the product");
    public static Error ActiveProduct
        => Error.Unexpected("Failed to active product");
    public static Error ArchiveProduct
        => Error.Unexpected("Failed to archive product");
    public static Error GetProductById
        => Error.Unexpected("Failed to retrieve product");
    public static Error GetAllProduct
        => Error.Unexpected("Failed to retrieve products");

    public static Error GetSuggestions
        => Error.Unexpected("Failed to retrieve suggestions");
    public static Error SearchProducts
        => Error.Unexpected("Failed to retrieve products");

    public static Error ReIndexProducts
        => Error.Unexpected("Failed to reindex products");
}