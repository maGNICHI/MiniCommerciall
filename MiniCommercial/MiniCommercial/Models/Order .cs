namespace MiniCommercial.Models.Entities;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    public int ClientId { get; set; } // L'erreur sur .ClientId vient d'ici
    public Client? Client { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;
    public OrderStatus Status { get; set; } = OrderStatus.Brouillon;

    public decimal TotalHT { get; set; } // L'erreur sur .TotalHT
    public decimal TotalTTC { get; set; } // L'erreur sur .TotalTTC

    // L'erreur sur .OrderLines vient d'ici (S bien à la fin)
    public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}