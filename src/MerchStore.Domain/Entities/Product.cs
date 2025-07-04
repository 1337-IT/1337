using MerchStore.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace MerchStore.Domain.Entities;

public class Product : Entity<Guid>
{
    [BsonElement("name")]
    public string Name { get; private set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; private set; } = string.Empty;

    [BsonElement("price")]
    public decimal Price { get; private set; } = 0; // Now using decimal

    [BsonElement("stock")]
    public int StockQuantity { get; private set; } = 0;

    [BsonElement("imageUrl")]
    [BsonIgnoreIfNull]
    public Uri? ImageUrl { get; private set; } = null;

    // Required for EF Core / MongoDB
    public Product() { }


    public Product(string name, string description, Uri? imageUrl, decimal price, int stockQuantity) : base(Guid.NewGuid())
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));
        if (name.Length > 100)
            throw new ArgumentException("Product name cannot exceed 100 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Product description cannot be empty", nameof(description));
        if (description.Length > 500)
            throw new ArgumentException("Product description cannot exceed 500 characters", nameof(description));

        if (imageUrl != null)
        {
            if (imageUrl.Scheme != "http" && imageUrl.Scheme != "https")
                throw new ArgumentException("Image URL must use HTTP or HTTPS protocol", nameof(imageUrl));
            if (imageUrl.AbsoluteUri.Length > 2000)
                throw new ArgumentException("Image URL exceeds maximum length of 2000 characters", nameof(imageUrl));

            string extension = Path.GetExtension(imageUrl.AbsoluteUri).ToLowerInvariant();
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!validExtensions.Contains(extension))
                throw new ArgumentException("Image URL must point to a valid image file", nameof(imageUrl));
        }

        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));
        if (stockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(stockQuantity));

        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void UpdateDetails(string name, string description, Uri? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (name.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        if (description.Length > 500)
            throw new ArgumentException("Description cannot exceed 500 characters", nameof(description));

        if (imageUrl != null)
        {
            if (imageUrl.Scheme != "http" && imageUrl.Scheme != "https")
                throw new ArgumentException("Image URL must use HTTP or HTTPS protocol", nameof(imageUrl));
            if (imageUrl.AbsoluteUri.Length > 2000)
                throw new ArgumentException("Image URL exceeds maximum length", nameof(imageUrl));

            string extension = Path.GetExtension(imageUrl.AbsoluteUri).ToLowerInvariant();
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!validExtensions.Contains(extension))
                throw new ArgumentException("Image URL must point to a valid image file", nameof(imageUrl));
        }

        Name = name;
        Description = description;
        ImageUrl = imageUrl;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative", nameof(newPrice));
        Price = newPrice;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(quantity));
        StockQuantity = quantity;
    }

    public bool DecrementStock(int quantity = 1)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (StockQuantity < quantity)
            return false;

        StockQuantity -= quantity;
        return true;
    }

    public void IncrementStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        StockQuantity += quantity;
    }
}
