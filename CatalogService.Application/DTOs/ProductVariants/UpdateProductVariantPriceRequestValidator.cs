namespace CatalogService.Application.DTOs.ProductVariants;

public sealed class UpdateProductVariantPriceRequestValidator : AbstractValidator<UpdateProductVariantPriceRequest>
{
    public UpdateProductVariantPriceRequestValidator()
    {
        RuleFor(pv => pv.Price)
            .NotNull()
            .GreaterThan(0);

        RuleFor(pv => pv.CompareAtPrice)
            .Custom((price, context) =>
            {
                if (price is null)
                    return;

                if (price.Value <= 0)
                    context.AddFailure("CompareAtPrice",
                        "the compare at price must be greater than 0");
            });

        RuleFor(pv => pv.Currency)
            .NotEmpty()
            .Length(3);
    }
}
