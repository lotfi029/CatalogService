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
    }
    

    public static CategoryVariantAttribute Create(
        Guid categoryId,
        Guid variantAttributeId,
        bool isRequired,
        short displayOrder,
        string? createdBy = null)
    {

        if (categoryId == Guid.Empty)
            throw new ArgumentException("'CategoryId' cannot be empty", nameof(categoryId));
        
        if (variantAttributeId == Guid.Empty)
            throw new ArgumentException("'VariantAttributeId' cannot be empty", nameof(variantAttributeId));

        return new(
            categoryId: categoryId,
            variantAttributeId: variantAttributeId,
            isRequired: isRequired,
            displayOrder: displayOrder,
            createdBy: createdBy);
    }

    public void UpdateDisplayOrder(short displayOrder)
        => DisplayOrder = displayOrder;

    public void MarkRequired() => IsRequired = true;
    public void MarkOptional() => IsRequired = false;
}