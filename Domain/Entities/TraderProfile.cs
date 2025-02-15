namespace Domain.Entities;

public class TraderProfile : AuditableEntity
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyAddress { get; set; } = string.Empty;
    public string CompanyPhone { get; set; } = string.Empty;
    public string CompanyEmail { get; set; } = string.Empty;
    public string CompanyWebsite { get; set; } = string.Empty;
    public string CompanyLogo { get; set; } = string.Empty;
    public string CompanyDescription { get; set; } = string.Empty;
    public string CompanyTaxNumber { get; set; } = string.Empty;
    public string CompanyType { get; set; } = string.Empty;
    public string CompanyLicense { get; set; } = string.Empty;
    public float WalletBalance { get; set; }
    public float Rating { get; set; }
    public bool IsVerified { get; set; }
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
}
