using CatalogService.Application.DTOs.ProductVariants;

namespace CatalogService.Application.DTOs.ProductCategories;

public sealed class ProductCategoryRequestValidator : AbstractValidator<ProductCategoryRequest>
{
    private bool _haveVariant = true;
    public ProductCategoryRequestValidator()
    {
        RuleFor(pc => pc.IsPrimary)
            .NotNull()
            .WithMessage("IsPrimary is required.");

        RuleFor(pc => pc.CategoryVariants)
            .Custom((categoryVariant, context) =>
            {
                if (categoryVariant is null)
                {
                    _haveVariant = false;
                    return;
                }
                else
                {
                    if (categoryVariant.Count == 0)
                        context.AddFailure(nameof(categoryVariant),
                            "CategoryVariants must contain at least one item.");
                }
            });

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
            }).When(pc => pc.CategoryVariants is not null && pc.CategoryVariants.Count > 0 && _haveVariant);

        RuleForEach(pc => pc.CategoryVariants)
            .SetValidator(new ProductVariantRequestValidator())
            .When(pc => _haveVariant);
    }
}
