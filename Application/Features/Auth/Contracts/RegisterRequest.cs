namespace Application.Features.Auth.Contracts;
public record RegisterRequest(
    string Firstname,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string? Region,
    string? VisitorType
    );