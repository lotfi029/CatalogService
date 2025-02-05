namespace Application.Features.Auth.Contracts;

public record AuthenticationResponse (
    string AccessToken,
    long ExpiresOn,
    string TokenType
    );
