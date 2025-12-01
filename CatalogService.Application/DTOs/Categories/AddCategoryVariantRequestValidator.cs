namespace CatalogService.Application.DTOs.Categories;

public sealed class AddCategoryVariantRequestValidator : AbstractValidator<AddCategoryVariantRequest>
{
    public AddCategoryVariantRequestValidator()
    {
        RuleFor(cv => cv.VariantId)
            .NotEmpty();

        RuleFor(cv => cv.DisplayOrder)
            .NotEmpty()
            .GreaterThan((short)0)
            .WithMessage("'{PropertyName}' must be greater than 0");

        RuleFor(cv => cv.IsRequired)
            .NotEmpty();
    }
}
