using Application.Features.Categories.Contracts;

namespace Application.Features.Categories.Validators;
public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Product {PropertyName} is required.")
            .Length(3, 100).WithMessage("Product {PropertyName} must be between {MinLength} and {MaxLength} characters.");

        RuleFor(e => e.Description)
            .NotEmpty().WithMessage("Product {PropertyName} is required.")
            .Length(10, 1000).WithMessage("Product {PropertyName} must be between {MinLength} and {MaxLength} characters.");
    }
}
