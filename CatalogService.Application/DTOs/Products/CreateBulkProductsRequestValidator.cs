namespace CatalogService.Application.DTOs.Products;

internal sealed class CreateBulkProductsRequestValidator : AbstractValidator<CreateBulkProductsRequest>
{
    public CreateBulkProductsRequestValidator()
    {
        RuleForEach(p => p.Products)
            .SetValidator(new ProductRequestValidator());

        RuleFor(p => p.Products)
            .Custom((products, context) =>
            {
                if (products.Count == 0)
                {
                    context.AddFailure("Products",
                        "Products must not be empty");
                }

                if (products.Count > 50)
                {
                    context.AddFailure("Products",
                        "Products must not exceed the 50 per creation");
                }

                if (products.Count != products.Distinct().Count())
                {
                    context.AddFailure("Products",
                        "Product must be distinct");
                }
            }).When(p => p.Products is not null);
    }
}