namespace InmobarcoDocsAPI.Entities;

public class Contract {
    public int Id { get; set; }
    public DateOnly StartDateTenant { get; set; }
    public DateOnly EndDateTenant { get; set; }
    public DateOnly StartDateLandlord { get; set; }
    public DateOnly EndDateLandlord { get; set; }
    public int Rent { get; set; }
    public int Deposit { get; set; }
    public string? InsuranceCompany { get; set; }
    public string? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public required string ApartmentId { get; set; }
    public Apartment? Apartment { get; set; }
}
