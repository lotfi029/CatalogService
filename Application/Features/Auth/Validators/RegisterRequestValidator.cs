using Application.Features.Auth.Contracts;

namespace Application.Features.Auth.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(3, 100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(3, 10);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(3, 10);

        RuleFor(x => x.Region)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(3, 100);

        RuleFor(x => x.VisitorType)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(3, 100);

        // TODO: Password
    }
}