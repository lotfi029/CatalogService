using CatalogService.Domain.Enums;

namespace CatalogService.Application.DTOs.VariantAttributes;
public sealed class CreateVariantAttributeRequestValidator : AbstractValidator<CreateVariantAttributeRequest>
{
    public CreateVariantAttributeRequestValidator()
    {
        RuleFor(v => v.Code)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("The length of '{PropertyName}' must be less than or equal to {MaxLength} characters.");

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("The length of '{PropertyName}' must be less than or equal to {MaxLength} characters.");

        RuleFor(v => v.Datatype)
            .NotEmpty()
            .MaximumLength(10)
                .WithMessage("The length of '{PropertyName}' must be less than or equal to {MaxLength} characters.")
            .IsEnumName(typeof(ValuesDataType), caseSensitive: false)
                .WithMessage("'{PropertyValue}' is not a valid '{PropertyName}'. Allowed values are: select, text, boolean.");

        RuleFor(e => e.AffectedInventory)
            .NotNull()
            .WithMessage("'{PropertyName}' is required.");

        RuleFor(v => v)
            .Custom((request, context) =>
            {
                bool isSelect =
                    string.Equals(request.Datatype, "select", StringComparison.OrdinalIgnoreCase);

                bool hasValues = request.AllowedValues is not null &&
                                 request.AllowedValues.Values.Count != 0;

                if (isSelect && !hasValues)
                {
                    context.AddFailure(nameof(request.AllowedValues),
                        "'AllowedValues' must be provided when the datatype is 'select'.");
                }

                if (!isSelect && hasValues)
                {
                    context.AddFailure(nameof(request.AllowedValues),
                        "'AllowedValues' can only be provided when the datatype is 'select'.");
                }
            });

        RuleFor(v => v)
            .Custom((request, context) =>
            {
                bool isSelect =
                    string.Equals(request.Datatype, "select", StringComparison.OrdinalIgnoreCase);

                if (isSelect && request.AllowedValues is not null)
                {
                    var values = request.AllowedValues.Values;
                    if (values.Count != values.Distinct().Count())
                        context.AddFailure(nameof(request.AllowedValues),
                            "'AllowedValues' cannot be duplicated");

                    if (values.Count >= 50)
                        context.AddFailure(nameof(request.AllowedValues),
                            "'AllowedValues' cannot be more than 50");
                }
            });
    }
}