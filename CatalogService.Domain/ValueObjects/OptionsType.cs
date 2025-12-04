namespace CatalogService.Domain.ValueObjects;

public record OptionsType
{
    public string DataTypeName { get; }
    public ValuesDataType DataType { get; }

    public OptionsType(ValuesDataType dataType)
    {
        DataTypeName = dataType.ToString();
        DataType = dataType;
    }
}