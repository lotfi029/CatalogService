namespace CatalogService.Application.DTOs.Products;

public sealed class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .Length(5, 100)
            .WithMessage("{PropertyName} length must be between {MinLenght} and {MaxLength}");


        RuleFor(p => p.Description)
            .Must(d =>
            {
                if (d is null)
                    return true;

                return d.Length > 10;
            }).WithMessage("{PropertyName} must be gretare than 10 char");
    }
}