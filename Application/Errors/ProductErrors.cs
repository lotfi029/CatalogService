namespace Application.Errors;
public class ProductErrors
{
    public static readonly Error InvalidOperation
        = Error.BadRequest(nameof(InvalidOperation), "Invalid operation!");

    public static readonly Error NullException
        = Error.BadRequest(nameof(NullException), "Null Exception");

    public static readonly Error NotFound
        = Error.NotFound(nameof(NotFound), "Product not found!");

}
