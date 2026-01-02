namespace CatalogService.Domain.Entities;

public class ProductCategories
{
    public Guid ProductId { get; }
    public Guid CategoryId { get; }
    public bool IsPrimary { get; private set; }
    public DateTime CreatedAt { get; }
    public Product Product { get; } = default!;
    public Category Category { get; } = default!;
    public bool IsDeleted { get; private set; }
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
        IsDeleted = false;
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

    public Result MarkAsPrimary()
    {
        if (IsPrimary)
            return DomainErrors.ProductCategories.AlreadyPrimary;
        IsPrimary = true;
        return Result.Success();
    }
    public Result MarkAsUnPrimary()
    {
        if (!IsPrimary)
            return DomainErrors.ProductCategories.AlreadyNotPrimary;

        IsPrimary = false;
        return Result.Success();
    }
    public Result Deleted()
    {
        if (!IsPrimary)
            return DomainErrors.ProductCategories.AlreadyDeleted;

        IsDeleted = true;
        return Result.Success();
    }
    public Result Restore()
    {
        if (!IsDeleted)
            return DomainErrors.ProductCategories.AlreadyNotDeleted;

        IsDeleted = false;
        return Result.Success();
    }

}