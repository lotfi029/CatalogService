namespace CatalogService.Application.DTOs.Products;

public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .Length(5, 100)
            .WithMessage("{PropertyName} length must be between {MinLenght} and {MaxLength}");

        RuleFor(p => p.VendorId)
            .NotEmpty()
            .Length(15,450)
            .WithMessage("{PropertyName} length must be between {MinLenght} and {MaxLength}");

        RuleFor(p => p.Status)
            .NotEmpty()
            .Length(5, 11)
            .WithMessage("Product status should be in {Draft, Active, Inactive, and Archived}");

        RuleFor(p => p.Sku)
            .NotNull()
            .Length(8, 8)
            .WithMessage("{PropertyName} must of the size 8");

        RuleFor(p => p.Description)
            .Must(d =>
            {
                if (d is null)
                    return true;

                return d.Length < 50;
            }).WithMessage("{PropertyName} must be gretare than 10 char");


    }
}