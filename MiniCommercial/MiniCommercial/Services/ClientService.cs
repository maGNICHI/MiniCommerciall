using Microsoft.EntityFrameworkCore;
using MiniCommercial.Data;
using MiniCommercial.Models.Entities;

namespace MiniCommercial.Services;

public class ClientService : IClientService
{
    private readonly ApplicationDbContext _context;
    public ClientService(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Client>> GetAllAsync() => await _context.Clients.ToListAsync();

    public async Task<Client?> GetByIdAsync(int id) => await _context.Clients.FindAsync(id);

    public async Task<Client> CreateAsync(Client client)
    {
        client.CreatedAt = DateTime.Now;
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<bool> UpdateAsync(int id, Client client)
    {
        if (id != client.Id) return false;
        _context.Entry(client).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); return true; }
        catch { return false; }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return false;
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }
}