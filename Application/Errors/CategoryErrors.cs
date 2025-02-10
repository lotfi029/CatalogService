namespace Application.Errors;

public class CategoryErrors
{
    public static readonly Error InvalidOperation
        = Error.BadRequest(nameof(InvalidOperation), "Invalid operation for category!");

    public static readonly Error NullException
        = Error.BadRequest(nameof(NullException), "Category object cannot be null!");

    public static readonly Error NotFound
        = Error.NotFound(nameof(NotFound), "Category not found!");
}
