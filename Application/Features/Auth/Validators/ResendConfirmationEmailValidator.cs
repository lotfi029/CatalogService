namespace Application.Features.Auth.Validators;

public class ResendConfirmationEmailValidator : AbstractValidator<ResendConfirmEmailRequest>
{
    public ResendConfirmationEmailValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty();
    }
}
