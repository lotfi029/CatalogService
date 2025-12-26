namespace CatalogService.Application.DTOs.CategoryVariantAttributes;

public sealed class AddCategoryVariantBulkRequestValidator : AbstractValidator<AddCategoryVariantBulkRequest>
{
    public AddCategoryVariantBulkRequestValidator()
    {
        RuleForEach(v => v.Variants)
            .SetValidator(new AddCategoryVariantRequestValidator());

        RuleFor(v => v.Variants)
            .Custom((Variants, context) =>
            {
                var variantIds = Variants.Select(v => v.VariantId);
                
                var displayOrders = Variants.Select(d => d.DisplayOrder);
                
                var validDisplayOrder =
                    displayOrders.Count() == displayOrders.Distinct().Count();


                if (variantIds.Count() != variantIds.Distinct().Count())
                    context.AddFailure("VariantId",
                        "'Variants' you cannot add duplicate variant id");

                if (Variants.Count >= 50)
                    context.AddFailure("Variants",
                        "'Variants' the number of added variant must be less than or equal 50 variant");

                if (!validDisplayOrder)
                    context.AddFailure("DisplayOrder",
                        "'Variants' the display order for all variant must unique");

            }).When(v => v.Variants is not null);
    }
}
