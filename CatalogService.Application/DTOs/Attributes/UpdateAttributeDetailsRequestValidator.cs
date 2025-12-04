namespace CatalogService.Application.DTOs.Attributes;

public sealed class UpdateAttributeDetailsRequestValidator : AbstractValidator<UpdateAttributeDetailsRequest>
{
    public UpdateAttributeDetailsRequestValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("The length of '{PropertyName}' must be less than or equal to {MaxLength} characters.");

        RuleFor(at => at.IsFilterable)
            .NotNull();

        RuleFor(at => at.IsSearchable)
            .NotNull();
    }
}
