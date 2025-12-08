namespace CatalogService.Domain.ValueObjects;

public record VariantsType
{
    public string DataTypeName { get; }
    public VariantDataType DataType { get; }

    public VariantsType(VariantDataType dataType)
    {
        DataTypeName = dataType.ToString();
        DataType = dataType;
    }
}