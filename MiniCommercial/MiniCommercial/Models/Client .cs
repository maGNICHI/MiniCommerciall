namespace MiniCommercial.Models.Entities;

public class Client
{
    public int Id { get; set; } // L'erreur sur .Id vient d'ici
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}