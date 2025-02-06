using FluentValidation;

namespace Application.Features.Auth.Validators;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailValidator()
    {
        RuleFor(e => e.Code)
            .NotEmpty();

        RuleFor(e => e.UserId)
            .NotEmpty();
    }
}
