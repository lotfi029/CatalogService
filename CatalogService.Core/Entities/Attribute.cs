namespace CatalogService.Core.Entities;

public class Attribute : BaseEntity
{
    public string Name {  set; get; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Type {  get; set; } = string.Empty;

    public bool IsRequired { get; set; } = false;
    public bool IsFilterable { get; set; } = false;
    public bool IsSearchable { get; set; } = false;
    public short DisplayOrder { get; set; } = 0;

    // Json
    public string Options { get; set; } = string.Empty;
    public string ValidationRules { get; set; } = string.Empty;

}
