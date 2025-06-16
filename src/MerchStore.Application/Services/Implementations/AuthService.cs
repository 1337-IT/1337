using MerchStore.Application.Services.Interfaces;
using MerchStore.Domain.Entities;
using MerchStore.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MerchStore.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;

    public AuthService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepo.GetByUsernameAsync(username);
        if (user is null) return null;

        var hash = HashPassword(password);
        return user.PasswordHash == hash ? user : null;
    }

    // ✅ Updated: returns bool to indicate success/failure
    public async Task<bool> RegisterUserAsync(string username, string password, string role)
    {
        var existing = await _userRepo.GetByUsernameAsync(username);
        if (existing is not null) return false;

        var user = new User(Guid.NewGuid().ToString())
    {
    Username = username,
    PasswordHash = HashPassword(password),
    Role = role
    };


        await _userRepo.AddUserAsync(user);
        return true;
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
