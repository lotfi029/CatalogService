namespace Domain.Entities;

public class DriverProfile
{
    public string Id { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public string VehiclePlateNumber { get; set; } = string.Empty;
    public string VehicleColor { get; set; } = string.Empty;
    public string VehicleLicense { get; set; } = string.Empty;
    public string DrivingLicense { get; set; } = string.Empty;
    public float WalletBalance { get; set; }
    public float TotalDiscount { get; set; }
    public float Rating { get; set; }
    public int TotalTrips { get; set; }
    public int TotalEarnings { get; set; }
    public bool IsVerified { get; set; } = false;
    public bool Status { get; set; } = true;
}
