namespace CatalogService.Application.DTOs.ProductCategories;

public sealed class ProductCategoryRequestValidator : AbstractValidator<ProductCategoryRequest>
{
    public ProductCategoryRequestValidator()
    {
        RuleFor(pc => pc.IsPrimary)
            .NotNull()
            .WithMessage("IsPrimary is required.");

        RuleFor(pc => pc.CategoryVariants)
            .NotNull().WithMessage("CategoryVariants is required.")
            .NotEmpty().WithMessage("CategoryVariants must contain at least one item.");

        RuleFor(pc => pc.CategoryVariants)
            .Custom((categoryVariants, context) =>
            {
                if (categoryVariants is null) return;

                //context.AddFailure("CategoryVariant", "CategoryVariants must contain at least one item.");

                if (categoryVariants.Count != categoryVariants.Distinct().Count())
                {
                    context.AddFailure("CategoryVariants",
                        "CategoryVariants contains duplicate entries");
                }
                if (categoryVariants.Count > 50)
                {
                    context.AddFailure("CategoryVariants",
                        "CategoryVariant must not exceed 50 item");
                }
            }).When(pc => pc.CategoryVariants is not null && pc.CategoryVariants.Count > 0);

        RuleForEach(pc => pc.CategoryVariants)
            .Custom((variantOption, context) =>
            {
                if (variantOption is null)
                {
                    context.AddFailure("CategoryVariants", "Variant item cannot be null.");
                    return;
                }

                if (variantOption.Variants is null || variantOption.Variants.Count == 0)
                {
                    context.AddFailure("Variants",
                        "Variants must not be null or empty.");
                    return;
                }

                if (variantOption.Variants.Count != variantOption.Variants.Distinct().Count())
                {
                    context.AddFailure("Variants",
                        "Variants must not contain duplicated values.");
                }

                if (variantOption.Variants.Count > 50)
                {
                    context.AddFailure("Variants",
                        "Variants must not exceed 50 items.");
                }
            });
    }
}
