using MerchStore.Domain.Entities;

namespace MerchStore.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddUserAsync(User user);
}
