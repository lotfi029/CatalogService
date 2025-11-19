using CatalogService.Domain.Abstractions;

namespace CatalogService.Domain.Entities;

public sealed class Attribute : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public AttibuteType Type {  get; private set; }

    public bool IsFilterable { get; private set; } = false;
    public bool IsSearchable { get; private set; } = false;


    // Json
    public Dictionary<string, object>? Options { get; private set; }
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
