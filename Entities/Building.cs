namespace InmobarcoDocsAPI.Entities;

public class Building {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
}
