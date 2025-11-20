using FluentValidation;

namespace CatalogService.Application.DTOs.Categories;

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .Length(5, 1000);

        RuleFor(c => c.Slug)
            .NotEmpty()
            .Length(5, 1000);

        RuleFor(c => c.Description)
            .Must(x =>
            {
                if (x is null)
                    return true;

                if (x.Length < 10)
                    return true;

                return false;
            }).WithMessage("{PropertyName} Must be greater than or equal 10");


        RuleFor(c => c.ParentId)
            .Must(x =>
            {
                if (x is null) return true;

                if (x.Value == Guid.Empty)
                    return false;

                return true;

            }).WithMessage("{PropertyName} must be valid GUID");

    }
}
