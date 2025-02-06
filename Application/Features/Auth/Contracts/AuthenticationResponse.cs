using Microsoft.VisualBasic;

namespace Application.Features.Auth.Contracts;

public record AuthenticationResponse (
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string AccessToken,
    long ExpiresOn,
    string TokenType,
    string RefreshToken,
    DateTime RefreshTokenExpiration
    );
