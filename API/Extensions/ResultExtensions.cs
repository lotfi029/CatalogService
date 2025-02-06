using Domain.Abstractions;
using Domain.Enums;

namespace API.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("there is an error occure");

        var errorExtensions = new Dictionary<string, object?>()
        {
            {
                "Errors",  new[]
                {
                    result.Error.Code,
                    result.Error.Details
                }
            }
        };

        return TypedResults.Problem(statusCode: GetStatusCode(result.Error.ErrorType),extensions: errorExtensions);

        static int GetStatusCode(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.BadRequest => StatusCodes.Status400BadRequest,
                ErrorType.UnAutherization => StatusCodes.Status401Unauthorized,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
    }
}
