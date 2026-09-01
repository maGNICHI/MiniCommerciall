using MiniCommercial.Models.DTOs;
using MiniCommercial.Models.Entities;
namespace MiniCommercial.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
    Task<OrderResponseDto?> GetOrderByIdAsync(int id);
    Task<OrderResponseDto> CreateOrderAsync(OrderCreateDto dto);
    Task<bool> UpdateOrderAsync(int id, OrderCreateDto dto);
    Task<bool> DeleteOrderAsync(int id);
    Task ValidateOrderAsync(int id);
    Task<DashboardStatsDto> GetDashboardStatsAsync(DateTime? startDate, DateTime? endDate);
}