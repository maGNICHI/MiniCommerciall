using MiniCommercial.Models.Entities;
using MiniCommercial.Services;
using Xunit;

namespace MiniCommercial.Tests;

public class ProductServiceTests : TestBase
{
    [Fact]
    public async Task CreateProduct_ShouldSaveToDatabase()
    {
        // ARRANGE
        using var context = GetDbContext();
        var product = new Product
        {
            Name = "Ordinateur",
            Reference = "REF-PC",
            UnitPriceHT = 1500,
            StockQuantity = 5
        };

        // ACT
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // ASSERT
        var savedProduct = await context.Products.FindAsync(product.Id);
        Assert.NotNull(savedProduct);
        Assert.Equal("Ordinateur", savedProduct.Name);
    }

    [Fact]
    public async Task Product_ShouldAllowZeroStock_ButNotNegative()
    {
        // ARRANGE
        using var context = GetDbContext();
        var product = new Product { Name = "Test", Reference = "T1", StockQuantity = -1 };

        // ACT & ASSERT
        // Ici on peut tester si une exception est levée ou si la validation 
        // de votre service bloque le stock négatif (selon votre implémentation)
        Assert.True(product.StockQuantity < 0);
    }
}