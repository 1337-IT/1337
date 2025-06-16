using MerchStore.Domain.Entities;

namespace MerchStore.Application.Services.Interfaces;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(string username, string password);
    Task<bool> RegisterUserAsync(string username, string password, string role); // ✅ Must return bool
}
