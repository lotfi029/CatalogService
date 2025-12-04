namespace CatalogService.Application.DTOs.Attributes;

public sealed class UpdateAttributeOptionRequestValidator : AbstractValidator<UpdateAttributeOptionRequest>
{
    public UpdateAttributeOptionRequestValidator()
    {
        RuleFor(a => a.Option)
            .NotEmpty()
            .Custom((options, context) =>
            {
                var values = options.Values;

                if (values.Count == 0)
                    context.AddFailure("Options",
                        "Option cannot be empty");

                if (values.Count != values.Distinct().Count())
                    context.AddFailure("Options",
                        "'Options' cannot be duplicated");

                if (values.Count >= 50)
                    context.AddFailure("Options",
                        "'Options' cannot be more than 50");
            }).When(a => a.Option is not null);
    }
}