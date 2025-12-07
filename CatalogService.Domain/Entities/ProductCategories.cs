namespace CatalogService.Domain.Entities;

public class ProductCategories
{
    public Guid ProductId { get; }
    public Guid CategoryId  { get; }
    public bool IsPrimary { get; private set; }
    public DateTime CreatedAt { get; }
    public Product Product { get; } = default!;
    public Category Category { get; } = default!;

    private ProductCategories() { }
    private ProductCategories(
        Guid productId,
        Guid categoryId,
        bool isPrimary)
    {
        ProductId = productId;
        CategoryId = categoryId;
        IsPrimary = isPrimary;
        CreatedAt = DateTime.UtcNow;
    }

    public static ProductCategories Create(
        Guid productId,
        Guid categoryId,
        bool isPrimary)
    {
        return new ProductCategories(
            productId,
            categoryId,
            isPrimary);
    }

    public void MarkAsPrimary()
    {
        IsPrimary = true;
    }
    public void MarkAsUnPrimary()
    {
        IsPrimary = false;
    }
}