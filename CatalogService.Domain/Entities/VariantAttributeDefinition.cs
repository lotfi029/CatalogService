using CatalogService.Domain.DomainEvents.VariantAttribute;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.Entities;

public class VariantAttributeDefinition : AuditableEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public VariantDatatype Datatype { get; private set; } = default!;
   
    public bool AffectsInventory { get; private set; } = false;

    public AllowedValuesJson? AllowedValues { get; private set; }

    private readonly List<CategoryVariantAttribute> _categories = [];
    public IReadOnlyCollection<CategoryVariantAttribute> CategoryVariantAttributes => _categories.AsReadOnly();


    private VariantAttributeDefinition() { }
    private VariantAttributeDefinition(
        string code,
        string name,
        VariantDatatype datatype,
        bool affectsInventory,
        AllowedValuesJson? allowedValues
        ) 
    {
        Code = code;
        Name = name;
        Datatype = datatype;
        AffectsInventory = affectsInventory;
        AllowedValues = allowedValues;
    }

    public static VariantAttributeDefinition Create(
        string code,
        string name,
        VaraintAttributeDatatype datatype,
        bool affectsInventory,
        AllowedValuesJson? allowedValues
        )
    {
        var validAllowedValues = VerifyAllowedValues(datatype, allowedValues);

        var variantAttribute =  new VariantAttributeDefinition(
            code: code,
            name: name,
            datatype: new VariantDatatype(datatype),
            affectsInventory: affectsInventory,
            allowedValues: validAllowedValues);

        variantAttribute.AddDomainEvent(new VariantAttributeCreatedDomainEvent(Id: variantAttribute.Id));

        return variantAttribute;
    }

    public void Update(string name, AllowedValuesJson? allowedValues)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        AllowedValues = VerifyAllowedValues(datatype: Datatype.Datatype, allowedValues: allowedValues);

        Name = name;

        AddDomainEvent(new VariantAttributeUpdatedDomainEvent(Id));
    }
    private static AllowedValuesJson VerifyAllowedValues(VaraintAttributeDatatype datatype, AllowedValuesJson? allowedValues)
    {
        if (allowedValues is not null)
        {
            if (datatype != VaraintAttributeDatatype.Select)
                throw new ArgumentException("Allowed Values valid only with select data type", nameof(allowedValues));
            var values = allowedValues.Values;
            if (values.Count == 0)
                throw new ArgumentException("Allowed Values cannot be empty", nameof(allowedValues));
            if (values.Count != values.Distinct().Count())
                throw new ArgumentException("values cannot be duplicated", nameof(allowedValues));
            if (values.Count >= 50)
                throw new ArgumentException("Allowed Values cannot accept more than 50 values", nameof(allowedValues));
        }
        else
        {
            if (datatype == VaraintAttributeDatatype.Select)
                throw new ArgumentException("allowed values Must be not empty with select datatype", nameof(allowedValues));
        }

        return allowedValues!;
    }
}