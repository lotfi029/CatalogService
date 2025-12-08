using CatalogService.Domain.JsonProperties;

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

    

    public static Error AddProductCategory =>
        Error.Unexpected("Failed to add product category");
}



public class ProductVariantErrors
{
    private const string _code = "ProductVariants";
    public static Error VariantNotFound(Guid categoryId, string variant)
        => Error.NotFound(
            $"{_code}.{VariantNotFound}",
            $"the variant '{variant}' is not found in the category variant: {categoryId}");

    public static Error InvalidVariantValue(string variant, string value, HashSet<string> allowedValues)
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidVariantValue)}",
            $"the allowed values: '{string.Join(", ", allowedValues)}' of the variant: '{variant}' doesnot have this value {value}");

    public static Error InvalidBooleanValue(string variant, string value)
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidBooleanValue)}",
            $"The value: {variant} is not a valid boolean Please Enter a valid boolean value for variant '{variant}'");

}