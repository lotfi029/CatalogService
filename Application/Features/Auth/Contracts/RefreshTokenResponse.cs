namespace Application.Features.Auth.Contracts;

public record RefreshTokenResponse(
    string Token, 
    DateTime Expiration    
    );