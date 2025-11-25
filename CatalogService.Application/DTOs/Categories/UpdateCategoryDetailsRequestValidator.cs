namespace CatalogService.Application.DTOs.Categories;

public sealed class UpdateCategoryDetailsRequestValidator : AbstractValidator<UpdateCategoryDetailsRequest>
{
    public UpdateCategoryDetailsRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .Length(5, 200)
            .WithMessage("{PropertyName} should be between {MinLength} and {MaxLength}");

        RuleFor(x => x.Description)
            .Must(d =>
            {
                if (d is null)
                    return true;

                if (d.Length <= 10)
                    return false;

                return true;
            }).WithMessage("{PropertyName} length should be more than or equal to 10 char");
    }
}