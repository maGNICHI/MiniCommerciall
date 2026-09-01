namespace MiniCommercial.Models.Entities;

public class OrderLine
{
    public int Id { get; set; }
    public int ProductId { get; set; }

    // Le ! évite l'avertissement. Le ? dit qu'il peut être absent au début.
    public virtual Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}