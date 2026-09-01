using MiniCommercial.Models.DTOs;
using MiniCommercial.Models.Entities;
using MiniCommercial.Services;
using Xunit;

namespace MiniCommercial.Tests;

public class OrderServiceTests : TestBase
{
    [Fact]
    public async Task CreateOrder_ShouldCalculateTVA19Percent()
    {
        using var context = GetDbContext();
        var service = new OrderService(context);
        context.Products.Add(new Product { Id = 1, Name = "Produit", UnitPriceHT = 100, StockQuantity = 10, Reference = "R1" });
        await context.SaveChangesAsync();

        var dto = new OrderCreateDto { ClientId = 1, Lines = new List<OrderLineDto> { new OrderLineDto { ProductId = 1, Quantity = 2 } } };
        var result = await service.CreateOrderAsync(dto);

        Assert.Equal(200m, result.TotalHT); // 100 * 2
        Assert.Equal(238m, result.TotalTTC); // 200 * 1.19
    }

    [Fact]
    public async Task ValidateOrder_ShouldReduceStock()
    {
        using var context = GetDbContext();
        var service = new OrderService(context);
        var product = new Product { Id = 10, Name = "P10", StockQuantity = 10, UnitPriceHT = 10, Reference = "REF10" };
        context.Products.Add(product);
        var order = new Order { Id = 1, Status = OrderStatus.Brouillon, OrderLines = new List<OrderLine> { new OrderLine { ProductId = 10, Quantity = 4, UnitPrice = 10 } } };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        await service.ValidateOrderAsync(1);

        var updatedProduct = await context.Products.FindAsync(10);
        Assert.Equal(6, updatedProduct.StockQuantity); // 10 - 4
    }

    [Fact]
    public async Task ValidateOrder_ShouldThrowException_WhenStockIsInsufficient()
    {
        using var context = GetDbContext();
        var service = new OrderService(context);
        context.Products.Add(new Product { Id = 5, Name = "P5", StockQuantity = 2, UnitPriceHT = 10, Reference = "R5" });
        var order = new Order { Id = 2, Status = OrderStatus.Brouillon, OrderLines = new List<OrderLine> { new OrderLine { ProductId = 5, Quantity = 10, UnitPrice = 10 } } };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<Exception>(() => service.ValidateOrderAsync(2));
    }
}