using MiniCommercial.Models.Entities;

namespace MiniCommercial.Services;

public interface ITokenService
{
    string GenerateJwtToken(User user);
}