using Microsoft.EntityFrameworkCore;
using MiniCommercial.Data;
using MiniCommercial.Models.Entities;

namespace MiniCommercial.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) => await _context.Products.FindAsync(id);

    public async Task<Product> CreateAsync(Product p)
    {
        // Si aucune référence n'est saisie, on en génère une unique
        if (string.IsNullOrWhiteSpace(p.Reference))
        {
            p.Reference = "PRD-" + DateTime.Now.Ticks.ToString().Substring(10);
        }

        _context.Products.Add(p);
        await _context.SaveChangesAsync();
        return p;
    }

    public async Task<bool> UpdateAsync(int id, Product p)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing == null) return false;

        existing.Reference = p.Reference; // Ne pas oublier de mettre à jour la référence
        existing.Name = p.Name;
        existing.Description = p.Description;
        existing.UnitPriceHT = p.UnitPriceHT;
        existing.StockQuantity = p.StockQuantity;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}