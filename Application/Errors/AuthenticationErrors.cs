namespace Application.Errors;
public class AuthenticationErrors
{
    public static readonly Error InvalidCredinitails
        = Error.UnAutherization(nameof(InvalidCredinitails), "Invalid email or password");

    public static readonly Error DuplicatedEmail
        = Error.Conflict(nameof(DuplicatedEmail), "this email already exists");

    public static readonly Error IsDisableUser
        = Error.UnAutherization(nameof(IsDisableUser), "contact your adminstrator");

    public static readonly Error EmailNotConfirmed
        = Error.Conflict(nameof(EmailNotConfirmed), "your email is not confirmed");
    
    public static readonly Error DuplicatedEmailConfirmed
        = Error.BadRequest(nameof(DuplicatedEmailConfirmed), "your email is confirmed");

    public static readonly Error LockedUser
        = Error.UnAutherization(nameof(LockedUser), "you have intered password many times");

    public static readonly Error InvalidCode
        = Error.Conflict(nameof(InvalidCode), "Invalid code, trye agin");

    public static readonly Error InvalidToken
        = Error.Conflict(nameof(InvalidToken), "invalid token");

    public static readonly Error InvalidNewPassword
        = Error.Conflict(nameof(InvalidNewPassword), "this password same old password");

    public static readonly Error UserNotFound
        = Error.NotFound(nameof(UserNotFound), "user not found");

    public static readonly Error UnAutherizationAccess
        = Error.UnAutherization(nameof(UnAutherizationAccess), "you don't have access to this resource");
}
