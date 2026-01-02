namespace CatalogService.Domain.Errors;

public class ProductVariantErrors
{
    private const string _code = "ProductVariants";
    public static Error InvalidId
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidId)}",
            "invalid id");

    public static Error NotFound(Guid id)
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}",
            $"product variant with id: '{id}' not found");

    public static Error VariantNotFound(Guid categoryId, string variant)
        => Error.NotFound(
            $"{_code}.{VariantNotFound}",
            $"the variant '{variant}' is not found in the category variant: {categoryId}");
    public static Error ProductValueNotFound =>
        Error.NotFound(
            $"{_code}.{nameof(ProductValueNotFound)}",
            "Product variant value is not found");

    public static Error InvalidVariantValue(string variant, string value, HashSet<string> allowedValues)
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidVariantValue)}",
            $"the allowed values: '{string.Join(", ", allowedValues)}' of the variant: '{variant}' doesnot have this value {value}");

    public static Error InvalidBooleanValue(string variant, string value)
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidBooleanValue)}",
            $"The value: {value} is not a valid boolean Please Enter a valid boolean value for variant '{variant}'");
    public static Error InvalidVariants =>
        Error.Conflict(
            $"{_code}.{nameof(InvalidVariants)}",
            "One or more variant not exist");

    public static Error AddProductVariant =>
        Error.Unexpected("Failed to add product variant");
    public static Error UpdateProductVariant => 
        Error.Unexpected("Failed to update product variant");
    public static Error DeleteProductVariant => 
        Error.Unexpected("Failed to delete product variant");
    public static Error DeleteAllProductVariant => 
        Error.Unexpected("Failed to delete all product variant");
    public static Error UpdateProductVariantPrice => 
        Error.Unexpected("Failed to update product variant price");
    public static Error GetProductVariantById => 
        Error.Unexpected("Failed to retrieve product variant");
    public static Error GetProductVariantBySku => 
        Error.Unexpected("Failed to retrieve product variant");
    public static Error GetProductVariantByProductId => 
        Error.Unexpected("Failed to retrieve product variant");
}

