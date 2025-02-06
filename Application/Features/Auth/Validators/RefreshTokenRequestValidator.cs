namespace Application.Features.Auth.Validators;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(e => e.RefreshToken)
            .NotEmpty();

        RuleFor(e => e.Token)
            .NotEmpty();
    }
}