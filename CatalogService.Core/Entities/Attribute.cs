namespace CatalogService.Core.Entities;

public class Attribute : Entity
{
    public string Name {  set; get; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public AttibuteType Type {  get; set; }

    public bool IsFilterable { get; set; } = false;
    public bool IsSearchable { get; set; } = false;

    // Json
    public string Options { get; set; } = string.Empty;

    public ICollection<ProductAttributes> ProductAttributes { get; set; } = [];
}
