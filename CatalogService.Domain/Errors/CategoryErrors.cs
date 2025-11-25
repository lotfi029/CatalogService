namespace CatalogService.Domain.Errors;

public sealed class CategoryErrors
{
    private const string _code = "Category";
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

    public static Error InvalidParentId
        => Error.Conflict(
            $"{_code}.{nameof(InvalidParentId)}",
            $"invalid parent id");
}
