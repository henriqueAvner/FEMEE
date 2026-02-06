using FEMEE.Domain.Entities.Principal;

namespace FEMEE.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<string> GenerateTokenAsync(User user);
        Task<bool> ValidateTokenAsync(string token);
        Task<User?> GetUserFromTokenAsync(string token);
        Task<int> GetUserIdFromTokenAsync(string token);
    }
}
