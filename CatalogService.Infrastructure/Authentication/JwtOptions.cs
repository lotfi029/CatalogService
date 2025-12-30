using System.ComponentModel.DataAnnotations;

namespace CatalogService.Infrastructure.Authentication;

internal class JwtOptions
{
    public const string SectionName = "Jwt";
    [Required]
    public string Key { get; set; } = string.Empty;
    [Required]
    public string Issure { get; set; } = string.Empty;
    [Required]
    public string Audiance {  get; set; } = string.Empty;
    [Required]
    public int ExpiryMinutes { get; set; }
}
