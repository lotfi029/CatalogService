namespace Application.Features.Auth.Contracts;

public record ConfirmEmailRequest(string UserId, string Code);
