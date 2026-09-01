using Microsoft.EntityFrameworkCore;
using MiniCommercial.Data;
using MiniCommercial.Models.Entities;
using MiniCommercial.Models.DTOs;

namespace MiniCommercial.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private const decimal TVA = 0.19m;

    public OrderService(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
    {
        return await _context.Orders.Include(o => o.Client)
            .Select(o => MapToResponseDto(o)).ToListAsync();
    }

    public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
    {
        var order = await _context.Orders.Include(o => o.Client)
            .Include(o => o.OrderLines).ThenInclude(l => l.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        return order == null ? null : MapToResponseDto(order);
    }

    public async Task<OrderResponseDto> CreateOrderAsync(OrderCreateDto dto)
    {
        // Règle : Pas de commande sans client
        if (dto.ClientId <= 0) throw new Exception("Le client est obligatoire.");

        var order = new Order
        {
            ClientId = dto.ClientId,
            OrderNumber = "CMD-" + DateTime.Now.Ticks.ToString().Substring(10),
            Status = OrderStatus.Brouillon,
            OrderDate = DateTime.Now
        };

        foreach (var item in dto.Lines)
        {
            // Règle : Quantité > 0
            if (item.Quantity <= 0) throw new Exception("La quantité doit être supérieure à 0.");

            var product = await _context.Products.FindAsync(item.ProductId)
                ?? throw new Exception("Produit introuvable.");

            order.OrderLines.Add(new OrderLine
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.UnitPriceHT // On fige le prix au moment de la commande
            });
        }

        // Calcul des totaux (Règle 4)
        order.TotalHT = order.OrderLines.Sum(l => l.Quantity * l.UnitPrice);
        order.TotalTTC = order.TotalHT * (1 + TVA);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return MapToResponseDto(order);
    }

    public async Task ValidateOrderAsync(int id)
    {
        var order = await _context.Orders.Include(o => o.OrderLines).ThenInclude(l => l.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) throw new Exception("Commande introuvable.");
        if (order.Status != OrderStatus.Brouillon) throw new Exception("Seule une commande en brouillon peut être validée.");

        foreach (var line in order.OrderLines)
        {
            // Règle : Stock suffisant
            if (line.Product!.StockQuantity < line.Quantity)
                throw new Exception($"Stock insuffisant pour {line.Product.Name}.");

            // Règle : Mise à jour du stock lors de la validation
            line.Product.StockQuantity -= line.Quantity;
        }

        order.Status = OrderStatus.Validee;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        // 1. Il faut utiliser .Include(o => o.OrderLines) pour charger les lignes en mémoire
        var order = await _context.Orders
            .Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return false;

        // 2. Si vous avez configuré le "Cascade Delete" dans EF Core, 
        // supprimer l'order supprimera automatiquement les lignes.
        _context.Orders.Remove(order);

        // 3. Sauvegarder les changements
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateOrderAsync(int id, OrderCreateDto dto)
    {
        // 1. On récupère la commande existante
        var order = await _context.Orders
            .Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return false;

        // 2. MISE À JOUR DU CLIENT (L'ID envoyé par Angular)
        // Assurez-vous que cette ligne est présente et correcte :
        order.ClientId = dto.ClientId;

        // 3. Mise à jour du statut
        if (!string.IsNullOrEmpty(dto.Status) && Enum.TryParse<OrderStatus>(dto.Status, out var newStatus))
        {
            order.Status = newStatus;
        }

        // 4. Mise à jour des lignes (On remplace les anciennes par les nouvelles)
        _context.OrderLines.RemoveRange(order.OrderLines);

        foreach (var item in dto.Lines)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product != null)
            {
                order.OrderLines.Add(new OrderLine
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPriceHT
                });
            }
        }

        // 5. Recalcul des totaux
        order.TotalHT = order.OrderLines.Sum(l => l.Quantity * l.UnitPrice);
        order.TotalTTC = order.TotalHT * 1.19m;

        // 6. Sauvegarde
        await _context.SaveChangesAsync();
        return true;
    }
    private static OrderResponseDto MapToResponseDto(Order o) => new OrderResponseDto
    {
        Id = o.Id,
        OrderNumber = o.OrderNumber,
        ClientName = o.Client?.Name ?? "Inconnu",
        ClientEmail = o.Client?.Email,
        ClientPhone = o.Client?.Phone,
        ClientAddress = o.Client?.Address,
        OrderDate = o.OrderDate,
        Status = o.Status.ToString(),
        TotalHT = o.TotalHT,
        TotalTTC = o.TotalTTC,
        Lines = o.OrderLines.Select(l => new OrderLineDto
        {
            // Remplissez le ProductId ici aussi pour le Frontend
            ProductId = l.ProductId,
            ProductName = l.Product?.Name ?? "Produit inconnu",
            Quantity = l.Quantity,
            UnitPrice = l.UnitPrice
        }).ToList()
    };
    public async Task<DashboardStatsDto> GetDashboardStatsAsync(DateTime? startDate, DateTime? endDate)
    {
        var ordersQuery = _context.Orders.AsQueryable();
        var clientsQuery = _context.Clients.AsQueryable();

        // Appliquer le filtre par date si les dates sont fournies
        if (startDate.HasValue)
            ordersQuery = ordersQuery.Where(o => o.OrderDate >= startDate.Value);

        if (endDate.HasValue)
            ordersQuery = ordersQuery.Where(o => o.OrderDate <= endDate.Value);

        return new DashboardStatsDto
        {
            TotalClients = await clientsQuery.CountAsync(),
            TotalOrders = await ordersQuery.CountAsync(),
            TotalRevenue = await ordersQuery.SumAsync(o => o.TotalTTC),
            ProductsInStock = await _context.Products.SumAsync(p => p.StockQuantity)
        };
    }
}