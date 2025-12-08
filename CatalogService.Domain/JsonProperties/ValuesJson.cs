namespace CatalogService.Domain.JsonProperties;

public sealed record ValuesJson
{
    public HashSet<string> Values { get; set; } = [];
}
