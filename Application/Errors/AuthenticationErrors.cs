using Microsoft.AspNetCore.Http;

namespace Application.Errors;
public class AuthenticationErrors
{
    public static readonly Error InvalidCredinitails
        = new(nameof(InvalidCredinitails), "Invalid email or password", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedEmail
        = new(nameof(DuplicatedEmail), "this email already exists", StatusCodes.Status400BadRequest);

    public static readonly Error IsDisableUser
        = new(nameof(IsDisableUser), "contact your adminstrator", StatusCodes.Status401Unauthorized);

    public static readonly Error EmailNotConfirmed
        = new(nameof(EmailNotConfirmed), "your email is not confirmed", StatusCodes.Status400BadRequest);
    
    public static readonly Error EmailConfirmed
        = new(nameof(EmailConfirmed), "your email is confirmed", StatusCodes.Status400BadRequest);

    public static readonly Error LockedUser
        = new(nameof(LockedUser), "you have intered password many times", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidCode
        = new(nameof(InvalidCode), "Invalid code, trye agin", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidToken
        = new(nameof(InvalidToken), "invalid token", StatusCodes.Status400BadRequest);
}
