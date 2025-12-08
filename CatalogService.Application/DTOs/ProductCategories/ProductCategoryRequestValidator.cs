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
            .SetValidator(new ProductVariantRequestValidator());
    }
}
