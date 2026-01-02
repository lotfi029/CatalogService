namespace CatalogService.Application.DTOs.ProductVariants;

public sealed class VariantValueRequestValidator : AbstractValidator<VariantValueRequest>
{
    public VariantValueRequestValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty();

        RuleFor(x => x.Value)
            .NotEmpty()
            .Length(3, 450);
    }
}