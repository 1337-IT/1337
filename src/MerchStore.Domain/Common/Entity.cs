using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MerchStore.Domain.Common;

public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
{
    [BsonId]
    [BsonElement("id")] // MongoDB expects the field to be called "id"
    [BsonRepresentation(BsonType.String)] // Important for Cosmos DB string-based shard key
    public virtual TId Id { get; protected set; }

    protected Entity(TId id)
    {
        if (EqualityComparer<TId>.Default.Equals(id, default))
        {
            throw new ArgumentException("The entity ID cannot be the default value.", nameof(id));
        }
        Id = id;
    }

    // Required for EF Core (and MongoDB deserialization)
    #pragma warning disable CS8618
    protected Entity() { }
    #pragma warning restore CS8618

    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }

    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
