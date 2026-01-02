namespace CatalogService.Application.DTOs.ProductVariants;

public sealed class ProductVariantRequestValidator : AbstractValidator<ProductVariantRequest>
{
    public ProductVariantRequestValidator()
    {
        RuleFor(e => e.Price)
            .NotNull()
            .NotEmpty().WithMessage("Price must be greater than 0");

        RuleFor(e => e.CompareAtPrice)
            .Custom((price, context) =>
            {
                if (price is null)
                    return;

                price = price.Value;
                if (price <= 0)
                    context.AddFailure("CompareAtPrice",
                        "CompareAtPrice must to be greater than 0");
            });

        RuleFor(e => e.ProductId)
            .NotEmpty();

        RuleForEach(x => x.Variants)
            .SetValidator(new VariantValueRequestValidator());

        RuleFor(x => x.Variants)
            .NotEmpty()
            .Custom((variants, context) =>
            {
                if (variants is null)
                    return;

                var variantIds = variants.Select(x => x.VariantId);

                if (variantIds.Distinct().Count() != variantIds.Count())
                    context.AddFailure("Variants",
                        "'Variants' should be distinct");

                if (variants.Count() > 50)
                    context.AddFailure("Variants",
                        "'Variants' must be less than 50");

            });
    }
}