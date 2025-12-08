using CatalogService.Domain.DomainEvents.VariantAttribute;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.Entities;

public class VariantAttributeDefinition : AuditableEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public VariantsType Datatype { get; private set; } = default!;
    public bool AffectsInventory { get; private set; } = false;

    public ValuesJson? AllowedValues { get; private set; }

    public ICollection<CategoryVariantAttribute> CategoryVariantAttributes { get; } = [];


    private VariantAttributeDefinition() { }
    private VariantAttributeDefinition(
        string code,
        string name,
        VariantsType datatype,
        bool affectsInventory,
        ValuesJson? allowedValues
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
        VariantsType dataType,
        bool affectsInventory,
        ValuesJson? allowedValues
        )
    {
        VerifyAllowedValues(dataType, allowedValues);

        var variantAttribute =  new VariantAttributeDefinition(
            code: code,
            name: name,
            datatype: dataType,
            affectsInventory: affectsInventory,
            allowedValues: allowedValues);

        variantAttribute.AddDomainEvent(new VariantAttributeCreatedDomainEvent(Id: variantAttribute.Id));

        return variantAttribute;
    }

    public void Update(string name, ValuesJson? allowedValues)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        VerifyAllowedValues(dataType: Datatype, allowedValues: allowedValues);
        
        AllowedValues = allowedValues;
        Name = name;

        AddDomainEvent(new VariantAttributeUpdatedDomainEvent(Id));
    }
    public Result Deleted()
        => Deleted();
    private static void VerifyAllowedValues(VariantsType dataType, ValuesJson? allowedValues)
    {
        if (dataType.DataType == VariantDataType.Select)
        {
            if (allowedValues is null)
                throw new ArgumentException("AllowedValues must be provided for Select type");

            if (allowedValues.Values.Count == 0)
                throw new ArgumentException("AllowedValues cannot be empty");
        }
        else
        {
            if (allowedValues is not null)
                throw new ArgumentException("AllowedValues are only allowed for Select type");
        }
    }
}