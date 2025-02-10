global using Application.Features.Product.Contract;

namespace Application.Features.Product.Validators;
public class ProductRequestValiator : AbstractValidator<ProductRequest>
{
    public ProductRequestValiator()
    {
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Product {PropertyName} is required.")
            .Length(1, 100).WithMessage("Product {PropertyName} must be between 1 and 100 characters.");

        RuleFor(e => e.Description)
            .NotEmpty().WithMessage("Product {PropertyName} is required.")
            .Length(1, 1000).WithMessage("Product {PropertyName} must be between 1 and 1000 characters.");

        RuleFor(e => e.Quentity)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

        RuleFor(e => e.Price)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

        RuleFor(e => e.CategoryId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
