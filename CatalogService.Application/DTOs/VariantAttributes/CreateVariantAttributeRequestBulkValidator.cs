namespace CatalogService.Application.DTOs.VariantAttributes;

public sealed class CreateVariantAttributeRequestBulkValidator : AbstractValidator<CreateVariantAttributeBulkRequest>
{
    public CreateVariantAttributeRequestBulkValidator()
    {
        RuleForEach(v => v.Variants)
            .SetValidator(new CreateVariantAttributeRequestValidator());

        RuleFor(e => e.Variants)
            .Custom((Variants, context) =>
            {
                if (!Variants.Any())
                {
                    context.AddFailure(nameof(Variants),
                        "'Variants' must be not empty");
                }
                else
                {
                    var variantCount = Variants.Count();
                    var notDuplicated = variantCount == Variants.Distinct().Count();
                    var notCodeDuplicated = variantCount == Variants.Select(c => c.Code).Distinct().Count();

                    if (!notDuplicated)
                    {
                        context.AddFailure(nameof(Variants),
                            "'Variants' must be not duplicated");
                    }
                    else if (!notCodeDuplicated)
                    {
                        context.AddFailure("Code",
                            "'Code' must be not duplicated");
                    }
                    if (variantCount > 50)
                    {
                        context.AddFailure(nameof(Variants),
                            "'Variants' must be at most 50 variants");
                    }

                }
            })
            .When(v => v.Variants is not null);
    }
}
