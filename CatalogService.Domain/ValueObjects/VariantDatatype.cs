namespace CatalogService.Domain.ValueObjects;

public record VariantDatatype
{
    public string DatatypeName { get; set; } = string.Empty;
    public VaraintAttributeDatatype Datatype { get; set; }

    public VariantDatatype(VaraintAttributeDatatype datatype)
    {
        if (datatype == VaraintAttributeDatatype.UnAssign)
            throw new ArgumentException("Must specify the datatype of the variant attribute definition");

        DatatypeName = datatype.ToString();
        Datatype = datatype;
    }
}
