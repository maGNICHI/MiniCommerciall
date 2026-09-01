namespace MiniCommercial.Models.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalClients { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; } // Somme des TTC
        public int ProductsInStock { get; set; }
    }
}
