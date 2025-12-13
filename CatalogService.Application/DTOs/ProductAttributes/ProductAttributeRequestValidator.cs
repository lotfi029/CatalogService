namespace CatalogService.Application.DTOs.ProductAttributes;

internal sealed class ProductAttributeRequestValidator : AbstractValidator<ProductAttributeRequest>
{
    public ProductAttributeRequestValidator()
    {
        RuleFor(e => e.Value)
            .NotEmpty()
            .Length(3, 100);
    }
}