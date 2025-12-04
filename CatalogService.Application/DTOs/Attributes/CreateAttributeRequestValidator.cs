using CatalogService.Domain.Enums;

namespace CatalogService.Application.DTOs.Attributes;

public sealed class CreateAttributeRequestValidator : AbstractValidator<CreateAttributeRequest>
{
    public CreateAttributeRequestValidator()
    {
        RuleFor(v => v.Code)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("The length of '{PropertyName}' must be less than or equal to {MaxLength} characters.");

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("The length of '{PropertyName}' must be less than or equal to {MaxLength} characters.");

        RuleFor(v => v.OptionsType)
            .NotEmpty()
            .MaximumLength(10)
                .WithMessage("The length of '{PropertyName}' must be less than or equal to {MaxLength} characters.")
            .IsEnumName(typeof(ValuesDataType), caseSensitive: false)
                .WithMessage("'{PropertyValue}' is not a valid '{PropertyName}'. Allowed values are: select, text, boolean.");

        RuleFor(at => at.IsFilterable)
            .NotNull();
        
        RuleFor(at => at.IsSearchable)
            .NotNull();

        RuleFor(v => v)
            .Custom((request, context) =>
            {
                bool isSelect =
                    string.Equals(request.OptionsType, "select", StringComparison.OrdinalIgnoreCase);

                bool hasValues = request.Options is not null &&
                                 request.Options.Values.Count != 0;

                if (isSelect && !hasValues)
                {
                    context.AddFailure(nameof(request.Options),
                        "'Options' must be provided when the datatype is 'select'.");
                }

                if (!isSelect && hasValues)
                {
                    context.AddFailure(nameof(request.Options),
                        "'Options' can only be provided when the datatype is 'select'.");
                }
            });

        RuleFor(v => v)
            .Custom((request, context) =>
            {
                bool isSelect =
                    string.Equals(request.OptionsType, "select", StringComparison.OrdinalIgnoreCase);

                if (isSelect && request.Options is not null)
                {
                    var values = request.Options.Values;
                    if (values.Count != values.Distinct().Count())
                        context.AddFailure(nameof(request.Options),
                            "'Options' cannot be duplicated");

                    if (values.Count >= 50)
                        context.AddFailure(nameof(request.Options),
                            "'Options' cannot be more than 50");
                }
            });
    }

}