namespace CatalogService.Domain.ValueObjects;

public record SKUValueObject
{
    public string Value { get; set; }
    public SKUValueObject(string value)
    {
        Value = value;
    }
}




// outbox pattern