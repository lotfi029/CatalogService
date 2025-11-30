namespace CatalogService.Domain.JsonProperties;

public record AllowedValuesJson
{
    public List<string> Values { get; set; } = [];
}
