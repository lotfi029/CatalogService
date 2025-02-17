namespace Domain.Entities;

public class TraderProfile
{
    public string Id { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyPhone { get; set; }
    public string? CompanyEmail { get; set; }
    public string? CompanyWebsite { get; set; }
    public string? CompanyLogo { get; set; }
    public string? CompanyDescription { get; set; }
    public string? CompanyTaxNumber { get; set; }
    public string? CompanyType { get; set; }
    public string? CompanyLicense { get; set; }
    public float WalletBalance { get; set; }
    public float Rating { get; set; }
    public bool IsVerified { get; set; }
}
