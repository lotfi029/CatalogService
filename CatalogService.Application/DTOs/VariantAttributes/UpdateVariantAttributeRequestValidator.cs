namespace CatalogService.Application.DTOs.VariantAttributes;

public sealed class UpdateVariantAttributeRequestValidator : AbstractValidator<UpdateVariantAttributeRequest>
{
    public UpdateVariantAttributeRequestValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("The length of '{PropertyName}' must be less than or equal to {MaxLength} characters.");

        RuleFor(v => v.AllowedValues)
            .Custom((AllowedValues, context) =>
            {
                if (AllowedValues!.Values.Count == 0)
                {
                    context.AddFailure(nameof(AllowedValues),
                        "'AllowedValues' must be not empty");
                }
                else
                {
                    var values = AllowedValues.Values;

                    if (values.Count != values.Distinct().Count())
                    {
                        context.AddFailure(nameof(AllowedValues),
                            "'AllowedValues' cannot be duplicated");
                        
                        if (values.Count >= 50)
                            context.AddFailure(nameof(AllowedValues),
                                "'AllowedValues' cannot be more than 50");
                    }
                }
            }).When(v => v.AllowedValues is not null);
    }
}
