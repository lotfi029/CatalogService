using CatalogService.Domain.DomainEvents.VariantAttribute;

namespace CatalogService.Application.DTOs.ProductVariants;

public sealed class UpdateProductVariantCustomOptionsRequestValidator : AbstractValidator<UpdateProductVariantCustomOptionsRequest>
{
    public UpdateProductVariantCustomOptionsRequestValidator()
    {
        RuleFor(e => e.CustomVariant)
            .NotNull();


        RuleFor(e => e.CustomVariant)
            .Custom((CustomVariant, context) =>
            {
                if (CustomVariant is null)
                    return;

                if (CustomVariant.Variants is null || CustomVariant.Variants.Count == 0)
                {
                    context.AddFailure("Variants",
                        "Variants is required and connot be null or empty");
                    
                    return;
                }

                if (CustomVariant.Variants.Count != CustomVariant.Variants.Distinct().Count())
                {
                    context.AddFailure("CustomVariant",
                        "CustomVariant must not contain duplicated values.");
                }

                if (CustomVariant.Variants.Count > 50)
                {
                    context.AddFailure("CustomVariant",
                        "CustomVariant must not exceed 50 items.");
                }
            });

        RuleForEach(e => e.CustomVariant.Variants)
            .Custom((variant, context) =>
            {
                if (string.IsNullOrWhiteSpace(variant.Key))
                {
                    context.AddFailure("Key",
                        "Key is required and cannot be null or empty");
                }
                if (string.IsNullOrWhiteSpace(variant.Value))
                {
                    context.AddFailure("Value",
                        "Value is required and cannot be null or empty");
                }
            }).When(pv => pv.CustomVariant is not null);
    }
}