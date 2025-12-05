using CatalogService.Domain.Enums;

namespace CatalogService.Application.DTOs.Products;

internal sealed class UpdateProductStatusRequestValidator : AbstractValidator<UpdateProductStatusRequest>
{
    public UpdateProductStatusRequestValidator()
    {
        RuleFor(v => v.Status)
           .NotEmpty()
           .MaximumLength(10)
               .WithMessage("The length of '{PropertyName}' must be less than or equal to {MaxLength} characters.")
           .IsEnumName(typeof(ProductStatus), caseSensitive: false)
               .WithMessage("'{PropertyValue}' is not a valid '{PropertyName}'. Product Status are: draft, active, inactive, archive.");
    }
}
