namespace CatalogService.Domain.Entities;

public class Product : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string VendorId { get; set; } = string.Empty;
    public Sku SKU { get; set; } = default!;
    public ProductStatus Status { get; set; } = ProductStatus.Unspecified;
    public Dictionary<string, object>? Metadata {  get; set; }


    private readonly List<ProductVariant> _variants = [];
    private readonly List<ProductAttributes> _attributes = [];
    private readonly List<ProductCategories> _categories = [];


    public IReadOnlyCollection<ProductVariant> ProductVariants => _variants.AsReadOnly();
    public IReadOnlyCollection<ProductAttributes> ProductAttributes => _attributes.AsReadOnly();
    public IReadOnlyCollection<ProductCategories> ProductCategories => _categories.AsReadOnly();


    private Product() { }
    private Product(
        string name,
        string? description,
        string vendorId,
        Sku sku,
        ProductStatus status,
        Dictionary<string, object>? metadata = null
        ) 
        : base() 
    {
        Name = name;
        Description = description;
        VendorId = vendorId;
        SKU = sku;
        Status = status;
        Metadata = metadata;
    }
    public static Product Create(
        string name,
        string? description,
        string vendorId,
        Sku sku,
        ProductStatus status,
        Dictionary<string, object>? metadata = null
        )
    {
        if (status == ProductStatus.Unspecified)
            throw new ArgumentException("Product status cannot be unspecified.", nameof(status));

        return new Product(
            name,
            description,
            vendorId,
            sku,
            status,
            metadata
            );
        { };
    }
    
    public void UpdateDetails(string name, string? description)
    { 
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be null or empty.", nameof(name));

        Name = name;

        if (description is not null)
            Description = description;
    }
    public void AddCategory(ProductCategories category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (_categories.Any(e => e.CategoryId == category.CategoryId))
            throw new InvalidOperationException("Category already added to the product.");

        _categories.Add(category);
    }
    public void AddAttribute(ProductAttributes attribute)
    {
        // check if this attribute valid for this product or not
        ArgumentNullException.ThrowIfNull(attribute);
        if (_attributes.Any(e => e.AttributeId == attribute.AttributeId))
            throw new InvalidOperationException("Attribute already added to the product.");

        _attributes.Add(attribute);
    }
    public void AddVariant(ProductVariant variant)
    {
        // check if this variant valid for this product or not
        ArgumentNullException.ThrowIfNull(variant);
        if (_variants.Any(e => e.Id == variant.Id))
            throw new InvalidOperationException("Variant with the same SKU already exists for this product.");
        
        _variants.Add(variant);
    }
    #region product status
    public void ActiveProduct()
    {
        if (Status == ProductStatus.Active)
        {
            throw new InvalidOperationException("Product is already active.");
        }

        Status = ProductStatus.Active;
        Active();
    }
    public void DeactiveProduct()
    {
        if (Status == ProductStatus.Inactive)
            throw new InvalidOperationException("Product is already active.");

        Status = ProductStatus.Inactive;
        Deactive();
    }
    public void ArchiveProduct()
    {
        if (Status == ProductStatus.Archived)
        {
            throw new InvalidOperationException("Product is already archived.");
        }
        Status = ProductStatus.Archived;
    }
    #endregion
}
