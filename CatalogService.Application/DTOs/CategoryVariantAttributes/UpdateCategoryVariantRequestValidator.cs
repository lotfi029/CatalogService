namespace CatalogService.Application.DTOs.CategoryVariantAttributes;

public sealed class UpdateCategoryVariantRequestValidator : AbstractValidator<UpdateCategoryVariantRequest>
{
    public UpdateCategoryVariantRequestValidator()
    {
        
        RuleFor(cv => cv.DisplayOrder)
            .NotEmpty()
            .GreaterThan((short)0)
            .WithMessage("'{PropertyName}' must be greater than 0");

        RuleFor(cv => cv.IsRequired)
            .NotNull();
    }
}
