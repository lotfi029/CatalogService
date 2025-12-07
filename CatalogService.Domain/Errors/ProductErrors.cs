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
    //public static Error MustAddCategoryVariants()

    public static Error CreateProduct
        => Error.Unexpected("Failed to create product");
    public static Error CreateBulkProduct
        => Error.Unexpected("Failed to create bulk product");
    public static Error UpdateProductDetails
        => Error.Unexpected("Failed to update details of the product");
    public static Error UpdateProductStatus
        => Error.Unexpected("Failed to update status of the product");
    public static Error GetProductById
        => Error.Unexpected("Failed to retrieve product");
    public static Error GetAllProduct
        => Error.Unexpected("Failed to retrieve products");
}
