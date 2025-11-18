namespace CatalogService.Domain.Entities;

public class Attribute : AuditableEntity
{
    public string Name {  set; get; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public AttibuteType Type {  get; set; }

    public bool IsFilterable { get; set; } = false;
    public bool IsSearchable { get; set; } = false;

    // Json
    public Dictionary<string, object>? Options { get; set; }
    public ICollection<ProductAttributes> ProductAttributes { get; set; } = [];

    public Attribute() { }
    private Attribute(
        string name, 
        string code, 
        AttibuteType type, 
        bool isFilterable = false, 
        bool isSearchable = false,
        Dictionary<string, object>? options = null
        )
    {
        Name = name;
        Code = code;
        Type = type;
        IsFilterable = isFilterable;
        IsSearchable = isSearchable;
        Options = options;
    }

    public static Attribute Create(
        string name, 
        string code, 
        AttibuteType type, 
        bool isFilterable = false, 
        bool isSearchable = false,
        Dictionary<string, object>? options = null
        )
    {
        switch (type)
        {
            case AttibuteType.Select:
                break;

            default:
                options = null;
                break;
        }

        return new Attribute(
            name,
            code,
            type,
            isFilterable,
            isSearchable,
            options
            );
    }
    public void UpdateDetails(string name, bool isFilterable = false, bool isSearchable = false)
    {
        Name = name;
        IsFilterable = isFilterable;
        IsSearchable = isSearchable;
    }
    public void UpdateOptions(Dictionary<string, object> options)
    {
        Options = options;
    }
}
