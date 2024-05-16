namespace InmobarcoDocsAPI.Entities;

public class Landlord {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string IdType { get; set; }
    public required string IdNumber { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string BankAccountNumber { get; set; }
    public required string BankName { get; set; }
    public required string BankAccountType { get; set; }
}
