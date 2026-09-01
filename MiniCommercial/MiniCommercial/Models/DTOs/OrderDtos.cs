namespace MiniCommercial.Models.DTOs;

public class OrderResponseDto
{
    public int Id { get; set; }
    // Initialisez avec string.Empty pour éviter l'erreur de valeur Null
    public string OrderNumber { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string? ClientEmail { get; set; }
    public string? ClientPhone { get; set; }
    public string? ClientAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalHT { get; set; }
    public decimal TotalTTC { get; set; }
    public List<OrderLineDto> Lines { get; set; } = new();
}

public class OrderLineDto
{
    // AJOUTEZ CETTE PROPRIÉTÉ (Correction de l'erreur ProductId)
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class OrderCreateDto
{
    public int ClientId { get; set; }
    public string? Status { get; set; } = "Brouillon";
    public List<OrderLineDto> Lines { get; set; } = new();
}