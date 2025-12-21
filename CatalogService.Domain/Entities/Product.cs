using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Domain.Entities;

public class Product : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public Guid VendorId { get; }
    public ProductStatus Status { get; private set; }


    private readonly List<ProductVariant> _variants = [];
    private readonly List<ProductAttributes> _attributes = [];
    private readonly List<ProductCategories> _categories = [];


    public IReadOnlyCollection<ProductVariant> ProductVariants => _variants.AsReadOnly();
    public IReadOnlyCollection<ProductAttributes> Attributes => _attributes.AsReadOnly();
    public IReadOnlyCollection<ProductCategories> Categories => _categories.AsReadOnly();


    private Product() { }
    private Product(Guid id) 
        : base(id) { }
    private Product(
        string name,
        string? description,
        Guid vendorId,
        ProductStatus status
        )
        : base()
    {
        Name = name;
        Description = description;
        VendorId = vendorId;
        Status = status;

        AddDomainEvent(new ProductCreatedDomainEvent(Id));
    }
    public static Result<Product> Create(
        string name,
        string? description,
        Guid vendorId
        )
    {

        return new Product(
            name,
            description,
            vendorId,
            ProductStatus.Draft
            );
        { }
        ;
    }
    public static Product CreateProxy(Guid Id)
    {
        return new Product(Id);
    }
    public Result UpdateDetails(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DomainErrors.Null("Products.Name");

        Name = name;

        if (description is not null)
            Description = description;

        AddDomainEvent(new ProductDetailsUpdatedDomainEvent(Id));
        return Result.Success();
    }
    #region product status
    public Result Activate()
    {
        if (ProductStatus.Active == Status)
            return DomainErrors.Products.ProductAlreadyInStatus(Status.ToString());

        if (!CheckValidChange(ProductStatus.Active))
            return DomainErrors.Products.InvalidStatusTransaction(Status.ToString(), ProductStatus.Active.ToString());

        Active();
        Status = ProductStatus.Active;
        AddDomainEvent(new ProductActivatedDomainEvent(Id));
        return Result.Success();
    }
    public Result Inactivate()
    {
        if (ProductStatus.Inactive == Status)
            return DomainErrors.Products.ProductAlreadyInStatus(Status.ToString());

        if (!CheckValidChange(ProductStatus.Inactive))
            return DomainErrors.Products.InvalidStatusTransaction(Status.ToString(), ProductStatus.Active.ToString());

        base.Deactive();
        Status = ProductStatus.Inactive;
        AddDomainEvent(new ProductDeactivatedDomainEvent(Id));
        return Result.Success();
    }
    public Result Archive()
    {
        if (ProductStatus.Archive == Status)
            return DomainErrors.Products.ProductAlreadyInStatus(Status.ToString());

        if (!CheckValidChange(ProductStatus.Archive))
            return DomainErrors.Products.InvalidStatusTransaction(Status.ToString(), ProductStatus.Active.ToString());
        base.Deactive();
        Status = ProductStatus.Archive;
        AddDomainEvent(new ProductArchivedDomainEvent(Id));
        return Result.Success();
    }
    public Result Draft()
        => DomainErrors.Products.InvalidStatusTransaction(Status.ToString(), ProductStatus.Draft.ToString());

    private bool CheckValidChange(ProductStatus newStatus)
    {
        return newStatus switch
        {
            ProductStatus.Draft => false,
            ProductStatus.Active => Status is ProductStatus.Draft or ProductStatus.Inactive,
            ProductStatus.Inactive => Status is ProductStatus.Draft or ProductStatus.Active,
            ProductStatus.Archive => Status is ProductStatus.Active or ProductStatus.Draft or ProductStatus.Inactive,
            _ => false
        };
    }
    #endregion
}
