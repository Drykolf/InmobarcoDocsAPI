namespace InmobarcoDocsAPI.Entities;

public class Tenant {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string IdType { get; set; }
    public required string IdNumber { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? CodeptorName { get; set; }
    public string? CodeptorIdType { get; set; }
    public string? CodeptorIdNumber { get; set; }
    public string? CodeptorEmail { get; set; }
    public string? CodeptorPhone { get; set; }

}
