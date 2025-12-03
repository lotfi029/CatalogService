namespace CatalogService.Application.DTOs.CategoryVariantAttributes;

public sealed class AddCategoryVariantRequestValidator : AbstractValidator<AddCategoryVariantRequest>
{
    public AddCategoryVariantRequestValidator()
    {
        RuleFor(cv => cv.VariantId)
            .NotEmpty();

        RuleFor(cv => cv.DisplayOrder)
            .NotNull()
            .GreaterThan((short)0);

        RuleFor(cv => cv.IsRequired)
            .NotNull();
    }
}
