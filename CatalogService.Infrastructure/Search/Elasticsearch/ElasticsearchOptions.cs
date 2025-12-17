using System.ComponentModel.DataAnnotations;

namespace CatalogService.Infrastructure.Search.ElasticSearch;

public sealed record ElasticsearchOptions
{
    public const string SectionName = "ElasticSettings";
    [Required]
    public string Uri { get; set; } = string.Empty;
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string DefaultIndex { get; set; } = string.Empty;
    [Required]
    public int NumberOfShards { get; set; }
    [Required]
    public int NumberOfReplicas { get; set; }
    [Required]
    public bool EnableDebugMode { get; set; }
    [Required]
    public int RequestTimeout { get; set; }
}
