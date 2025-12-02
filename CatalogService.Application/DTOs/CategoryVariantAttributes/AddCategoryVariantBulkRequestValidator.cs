namespace CatalogService.Application.DTOs.CategoryVariantAttributes;

public sealed class AddCategoryVariantBulkRequestValidator : AbstractValidator<AddCategoryVariantBulkRequest>
{
    public AddCategoryVariantBulkRequestValidator()
    {
        RuleForEach(v => v.Variants)
            .SetValidator(new AddCategoryVariantRequestValidator());

        RuleFor(v => v.Variants)
            .Custom((variant, context) =>
            {
                var variantIds = variant.Select(v => v.VariantId);
                var displayOrders = variant.Select(d => d.DisplayOrder);
                var validDisplayOrder =
                    displayOrders.Count() == displayOrders.Distinct().Count() &&
                    displayOrders.Min() == 1 && displayOrders.Max() == displayOrders.Count();

                if (variantIds.Count() != variantIds.Distinct().Count())
                    context.AddFailure(nameof(variantIds),
                        "'Variants' you cannot add duplicate variant id");

                if (variantIds.Count() >= 50)
                    context.AddFailure(nameof(variantIds),
                        "'Variants' the number of added variant must be less than or equal 50 variant");

                if (!validDisplayOrder)
                    context.AddFailure(nameof(variantIds),
                        "'Variants' the display order for all variant must be ordered and non duplicated");
            }).When(v => v.Variants is not null);
    }
}
