namespace CatalogService.Domain.Errors;

public class ProductAttributeErrors
{
    private const string _code = "productAttributes";
    public static Error InvalidId
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidId)}",
            $"Please enter a valid id");

    public static Error DuplicatedAttribute
        => Error.Conflict(
            $"{_code}.{nameof(DuplicatedAttribute)}",
            "this attribute already exist on this product");

    public static Error NotFound(Guid productId, Guid attributeId)
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}",
            $"the attribute with id: {attributeId} is not found in the product with id:{productId}");
    public static Error InvalidAttributeValue(string attribute, string value, HashSet<string> allowedValues)
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidAttributeValue)}",
            $"the allowed values: '{string.Join(", ", allowedValues)}' of the attribute: '{attribute}' doesnot have this value {value}");
    public static Error InvalidBooleanValue(string attribute, string value)
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidBooleanValue)}",
            $"The value: {value} is not a valid boolean Please Enter a valid boolean value for attribute '{attribute}'");
    public static Error AddProductAttribute
        => Error.Unexpected("Failed to add product attribute");
    public static Error AddProductAttributeBulk
        => Error.Unexpected("Failed to add (bulk) product attribute");
    public static Error UpdateProductAttribute
        => Error.Unexpected("Failed to update product attribute");
    public static Error DeleteProductAttribute
        => Error.Unexpected("Failed to delete product attribute");
    public static Error DeleteAllProductAttribute
        => Error.Unexpected("Failed to delete all product attribute");
    public static Error GetAllProductAttribute
        => Error.Unexpected("Failed to get all product attribute");
    public static Error GetProductAttribute
        => Error.Unexpected("Failed to get product attribute");
}