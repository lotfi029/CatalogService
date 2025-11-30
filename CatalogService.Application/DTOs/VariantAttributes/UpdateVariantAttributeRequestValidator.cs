namespace CatalogService.Application.DTOs.VariantAttributes;

public sealed class UpdateVariantAttributeRequestValidator : AbstractValidator<UpdateVariantAttributeRequest>
{
    public UpdateVariantAttributeRequestValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("The length of '{PropertyName}' must be less than or equal to {MaxLength} characters.");

        RuleFor(v => v)
            .Custom((request, context) =>
            {
                bool value = request.AllowedValues is not null &&
                                 request.AllowedValues.Values.Count == 0;

                if (value)
                {
                    context.AddFailure(nameof(request.AllowedValues),
                        "'AllowedValues' must be have values");
                }

            });
    }
}
