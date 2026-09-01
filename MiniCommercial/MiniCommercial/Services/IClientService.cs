using MiniCommercial.Models.Entities;
namespace MiniCommercial.Services;

public interface IClientService
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(int id);
    Task<Client> CreateAsync(Client client);
    Task<bool> UpdateAsync(int id, Client client);
    Task<bool> DeleteAsync(int id);
}