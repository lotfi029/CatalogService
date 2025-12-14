namespace CatalogService.Domain.Errors;

public sealed class CategoryVariantAttributeErrors
{
    private const string _code = nameof(CategoryVariantAttributeErrors);
    public static Error InvalidId
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidId)}",
            $"Please enter a valid id");

    public static Error AlreadyExists(Guid id, Guid variantId)
        => Error.Conflict(
            $"{_code}.{nameof(AlreadyExists)}",
            $"The Variant with id: {variantId} already exist in the category with id: {id}");

    public static Error NotFound(Guid id, Guid variantId)
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}",
            $"'VariantAttribute' with id '{variantId}' not found for the category: '{id}'");
    public static Error VariantNotFound(Guid categoryId)
        => Error.NotFound(
            $"{_code}.{nameof(VariantNotFound)}",
            $"there is no variants to this category: '{categoryId}'");
    public static Error FaildToAddVariantsToCategory(int number) =>
        Error.BadRequest(
            $"{_code}.{nameof(FaildToAddVariantsToCategory)}",
            $"Error Ocurred while adding: '{number}' of variants to category");

    public static Error AddCategoryVariantBulk
        => Error.Unexpected($"Error ocurred while adding bulk variant attribute to category");

    public static Error AddCategoryVariantAttribute
        => Error.Unexpected($"Error ocurred while adding variant attribute to category");

    public static Error GetCategoryVariantAttribute
        => Error.Unexpected("Error ocurred while retrieve category variant attribute");

    public static Error GetAllCategoryVariantAttribute
        => Error.Unexpected("Error ocurred while retrieve category variant attributes");

}