namespace CatalogService.Domain.Errors;

public sealed class CategoryErrors
{
    private const string _code = "Categories";
    public static Error ParentNotFound(Guid parentId)
        => Error.NotFound(
            $"{_code}.{nameof(ParentNotFound)}",
            $"Parent category with ID {parentId} does not exist");

    public static Error SlugAlreadyExist(string slug)
        => Error.NotFound(
            $"{_code}.{nameof(SlugAlreadyExist)}",
            $"Category with slug: '{slug}' is already exist");

    public static Error NotFound(Guid id)
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}",
            $"Category with id: '{id}' does not exist");

    public static Error InvalidId
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidId)}",
            $"Please enter a valid id");
    
    public static Error InvalidSlug
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidSlug)}",
            $"Please enter a valid slug");

    public static Error CannotMoveToSelf
        => Error.Conflict(
            $"{_code}.{nameof(CannotMoveToSelf)}",
            $"can't move to it self");

    public static Error InvalidChildToMoving
        => Error.Conflict(
            $"{_code}.{nameof(InvalidChildToMoving)}",
            "can't make move the category to subCategory of it self");

    public static Error InconsistentTreeStructure
        => Error.BadRequest(
            $"{_code}.{nameof(InconsistentTreeStructure)}",
            $"in consistent tree structure");

    public static Error AlreadyHasThisParent
        => Error.BadRequest(
            $"{_code}.{nameof(AlreadyHasThisParent)}",
            "Category Already Have This Parent");

    public static Error SlugNotFound(string slug)
        => Error.NotFound(
            $"{_code}.{nameof(SlugNotFound)}",
            $"Category with slug: '{slug}' does not exist");

    public static Error VariantAlreadyExist
        => Error.BadRequest(
            $"{_code}.{nameof(VariantAlreadyExist)}",
            "category have this variant already");

    public static Error CategoryVariantNotFound(Guid id, Guid variantId)
        => Error.NotFound(
            $"{_code}.{nameof(CategoryVariantNotFound)}",
            $"'VariantAttribute' with id '{variantId}' not found for the category: '{id}'");
}
