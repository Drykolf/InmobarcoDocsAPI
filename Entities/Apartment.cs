namespace InmobarcoDocsAPI.Entities;

public class Apartment {
    public int Id { get; set; }
    public required int Flat { get; set; }
    public string? Parking { get; set; }
    public string? Storage { get; set; }
    public int BuildingId { get; set; }
    public Building? Building { get; set; }
    public string? LandlordId { get; set; }
    public Landlord? Landlord { get; set; }
}
