namespace CatalogService.Application.DTOs.ProductCategories;

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

        RuleFor(e => e.Variants)
            .NotNull()
            .NotEmpty();


        RuleFor(e => e.Variants)
            .Custom((variants, context) =>
            {
                if (variants is null)
                    return;

                if (variants.Variants.Count != variants.Variants.Distinct().Count())
                {
                    context.AddFailure("Variants",
                        "Variants must not contain duplicated values.");
                }

                if (variants.Variants.Count > 50)
                {
                    context.AddFailure("Variants",
                        "Variants must not exceed 50 items.");
                }
            });

        RuleForEach(e => e.Variants.Variants)
            .Custom((variant, context) =>
            {
                if (string.IsNullOrWhiteSpace(variant.Key))
                {
                    context.AddFailure("Key",
                        "{PropertyName} is required and cannot be null or empty");
                }
                if (string.IsNullOrWhiteSpace(variant.Value))
                {
                    context.AddFailure("Value",
                        "{PropertyName} is required and cannot be null or empty");
                }
            });
    }
}