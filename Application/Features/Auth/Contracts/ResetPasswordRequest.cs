namespace Application.Features.Auth.Contracts;

public record ResetPasswordRequest(string Email, string ResetToken, string NewPassword);