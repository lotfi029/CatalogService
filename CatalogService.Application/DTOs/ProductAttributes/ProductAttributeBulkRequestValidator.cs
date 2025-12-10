namespace CatalogService.Application.DTOs.ProductAttributes;

public sealed class ProductAttributeBulkRequestValidator : AbstractValidator<ProductAttributeBulkRequest>
{
    public ProductAttributeBulkRequestValidator()
    {
        RuleFor(x => x.Attributes)
            .NotEmpty()
            .WithMessage("{PropertyName} is required");

        RuleFor(pa => pa.Attributes)
            .Must(HaveUniqueAttributeId)
            .When(pa => pa.Attributes is not null)
            .WithMessage("Duplicate attribute IDs found in batch");

        RuleForEach(pa => pa.Attributes)
            .Must(pa => pa.AttributeId != Guid.Empty)
            .WithMessage("Attribute Id connot be empty")
            .Must(pa => !string.IsNullOrWhiteSpace(pa.Value))
            .WithMessage("Attribute value connot be empty");
    }
    private bool HaveUniqueAttributeId(IEnumerable<ProductAttributeBulk> attributes)
    {
        var attributeIds = attributes.Select(a => a.AttributeId).ToList();
        return attributeIds.Count == attributeIds.Distinct().Count();
    }
}

