using CatalogService.Domain.DomainEvents.VariantAttribute;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.Entities;

public class VariantAttributeDefinition : AuditableEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public VariantDatatype Datatype { get; private set; } = default!;
   
    public bool AffectsInventory { get; private set; } = false;
    public bool AffectsPricing { get; private set; } = false; // deleted
    public short DisplayOrder { get; private set; } = 0; // deleted

    public AllowedValuesJson? AllowedValues { get; private set; }

    private readonly List<CategoryVariantAttribute> _categories = [];
    public IReadOnlyCollection<CategoryVariantAttribute> CategoryVariantAttributes => _categories.AsReadOnly();


    private VariantAttributeDefinition() { }
    private VariantAttributeDefinition(
        string code,
        string name,
        VariantDatatype datatype,
        bool affectsInventory,
        bool affectsPricing,
        short displayOrder,
        AllowedValuesJson? allowedValues
        ) 
    {
        Code = code;
        Name = name;
        Datatype = datatype;
        AffectsInventory = affectsInventory;
        AffectsPricing = affectsPricing;
        DisplayOrder = displayOrder;
        AllowedValues = allowedValues;
    }

    public static VariantAttributeDefinition Create(
        string code,
        string name,
        VaraintAttributeDatatype datatype,
        bool affectsInventory,
        bool affectsPricing,
        short diplayOrder,
        AllowedValuesJson? allowedValues
        )
    {
        if (datatype == VaraintAttributeDatatype.Select && allowedValues is null)
            throw new InvalidOperationException("Must be add the select values");

        var validAllowedValues = VerifyAllowedValues(datatype, allowedValues);

        var variantAttribute =  new VariantAttributeDefinition(
            code: code,
            name: name,
            datatype: new VariantDatatype(datatype),
            affectsInventory: affectsInventory,
            affectsPricing: affectsPricing,
            displayOrder: diplayOrder,
            allowedValues: validAllowedValues);

        variantAttribute.AddDomainEvent(new VariantAttributeCreatedDomainEvent(Id: variantAttribute.Id));

        return variantAttribute;
    }

    public void UpdateName(string name, AllowedValuesJson? allowedValues)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        AllowedValues = VerifyAllowedValues(datatype: Datatype.Datatype, allowedValues: allowedValues);

        Name = name;

        AddDomainEvent(new VariantAttributeNameUpdatedDomainEvent(Id));
    }
    private static AllowedValuesJson VerifyAllowedValues(VaraintAttributeDatatype datatype, AllowedValuesJson? allowedValues)
    {
        if (datatype != VaraintAttributeDatatype.Select && allowedValues is not null)
            throw new InvalidOperationException("'AllowedValues' valid only with select data type");

        var validAllowedValues =
            datatype == VaraintAttributeDatatype.Select &&
            allowedValues is not null &&
            allowedValues.Values.Count > 0;

        if (!validAllowedValues)
            throw new InvalidOperationException("'AllowedValues' can not be null or empty");

        return allowedValues!;
    }
}