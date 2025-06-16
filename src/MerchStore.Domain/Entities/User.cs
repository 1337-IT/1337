using MerchStore.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace MerchStore.Domain.Entities;

public class User : Entity<string>
{
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "User";

    public User(string id) : base(id) { }

    [BsonConstructor]
    public User(string id, string username, string passwordHash, string role) : base(id)
    {
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
    }

    // Required for deserialization
    protected User() : base(Guid.NewGuid().ToString()) { }
}
