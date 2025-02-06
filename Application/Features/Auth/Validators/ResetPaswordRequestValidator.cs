namespace Application.Features.Auth.Validators;

public class ResetPaswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPaswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.NewPassword)
            .NotEmpty();

        RuleFor(x => x.ResetToken)
            .NotEmpty();
    }
}
