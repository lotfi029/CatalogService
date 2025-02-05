namespace Application.Features.Auth.Contracts;

public record BearerTokenResponse(
    string AccessToken,
    long ExpiresIn,
    string TokenType = "Bearer Token"
    );
