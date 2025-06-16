using MerchStore.Domain.Entities;
using MerchStore.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace MerchStore.Infrastructure.Persistence.Repositories;

public class CosmosUserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public CosmosUserRepository(IConfiguration config)
    {
        var connectionString = config["CosmosDbSettings:ConnectionString"];
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(config["CosmosDbSettings:DatabaseName"]);
        _users = db.GetCollection<User>("Users");
    }

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _users.Find(u => u.Username == username).FirstOrDefaultAsync();

    public async Task AddUserAsync(User user) =>
        await _users.InsertOneAsync(user);
}
