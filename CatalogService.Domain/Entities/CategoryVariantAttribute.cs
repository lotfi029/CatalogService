namespace CatalogService.Domain.Entities;

public class CategoryVariantAttribute
{
    public Guid CategoryId { get; private init; }
    public Guid VariantAttributeId { get; private init; }
    public bool IsRequired { get; private set; }
    public short DisplayOrder
    {
        get;
        private set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(DisplayOrder), "'DisplayOrder' cannot be negative or equal to 0");

            field = value;
        }
    }
    public DateTime CreatedAt { get; }
    public string? CreatedBy { get; }
    public bool IsDeleted { get; private set; }
    public Category Category { get; } = default!;
    public VariantAttributeDefinition VariantAttribute { get; } = default!;

    private CategoryVariantAttribute() { }
    private CategoryVariantAttribute(
        Guid categoryId,
        Guid variantAttributeId,
        short displayOrder,
        bool isRequired = false,
        string? createdBy = null)
    {
        CategoryId = categoryId;
        VariantAttributeId = variantAttributeId;
        IsRequired = isRequired;
        DisplayOrder = displayOrder;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
        IsDeleted = false;
    }


    public static Result<CategoryVariantAttribute> Create(
        Guid categoryId,
        Guid variantAttributeId,
        bool isRequired,
        short displayOrder,
        string? createdBy = null)
    {

        if (categoryId == Guid.Empty)
            return DomainErrors.Null(nameof(CategoryId));

        if (variantAttributeId == Guid.Empty)
            return DomainErrors.Null(nameof(VariantAttributeId));

        return new CategoryVariantAttribute(
            categoryId: categoryId,
            variantAttributeId: variantAttributeId,
            isRequired: isRequired,
            displayOrder: displayOrder,
            createdBy: createdBy);
    }

    public void UpdateDisplayOrder(short displayOrder)
        => DisplayOrder = displayOrder;

    public Result MarkRequired()
    {
        if (IsRequired)
            return DomainErrors.CategoryVariantAttributes.AlreadyRequired;

        IsRequired = true;
        return Result.Success();
    }
    public Result MarkOptional()
    {
        if (IsRequired)
            return DomainErrors.CategoryVariantAttributes.AlreadyNotRequired;

        IsRequired = false;
        return Result.Success();
    }
    public Result Delete()
    {
        if (IsDeleted)
            return DomainErrors.CategoryVariantAttributes.AlreadyDeleted;

        IsDeleted = true;
        return Result.Success();
    }
    public Result Restore()
    {
        if (!IsDeleted)
            return DomainErrors.CategoryVariantAttributes.AlraedyNotDeleted;

        IsDeleted = false;
        return Result.Success();
    }
}