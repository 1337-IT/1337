using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MerchStore.Domain.Entities;
using MerchStore.Domain.ValueObjects;

namespace MerchStore.Infrastructure.Persistence;

/// <summary>
/// Class for seeding the database with initial data.
/// This is useful for development, testing, and demos.
/// </summary>
public class AppDbContextSeeder
{
    private readonly ILogger<AppDbContextSeeder> _logger;
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor that accepts the context and a logger
    /// </summary>
    /// <param name="context">The database context to seed</param>
    /// <param name="logger">The logger for logging seed operations</param>
    public AppDbContextSeeder(AppDbContext context, ILogger<AppDbContextSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    public virtual async Task SeedAsync()
    {
        try
        {
            // Ensure the database is created (only needed for in-memory database)
            // For SQL Server, you would use migrations instead
            await _context.Database.EnsureCreatedAsync();

            // Seed products if none exist
            await SeedProductsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    /// <summary>
    /// Seeds the database with sample products
    /// </summary>
    private async Task SeedProductsAsync()
    {
        // Check if we already have products (to avoid duplicate seeding)
        if (!await _context.Products.AnyAsync())
        {
            _logger.LogInformation("Seeding products...");

            // Add sample products
            var products = new List<Product>
            {
                new Product(
                    "Casual Sports Cap - Green Edition",
                    "A comfortable cap suits all weather conditions.",
                    // new Uri("https://example.com/images/tshirt.jpg"),
                    new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap1.jpg"),
                    Money.FromSEK(99.00m),
                    10),

                new Product(
                    "Casual Sports Cap - Grey Edition",
                    "A comfortable cap suits all weather conditions.",
                    // new Uri("https://example.com/images/mug.jpg"),
                    new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap2.jpg"),
                    Money.FromSEK(109.00m),
                    20),

                new Product(
                    "Stylish Sports Cap - Fire Edition",
                    "A Confortable cap suits while going out for Sport events.",
                    // new Uri("https://example.com/images/stickers.jpg"),
                    new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap3.jpg"),
                    Money.FromSEK(99.00m),
                    0),

                new Product(
                    "Stylish Sports Cap - Minimalist Design",
                    "A Confortable cap suits while going out for events.",
                    // new Uri("https://example.com/images/hoodie.jpg"),
                    new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap4.jpg"),
                    Money.FromSEK(109.99m),
                    15)
            };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Product seeding completed successfully.");
        }
        else
        {
            _logger.LogInformation("Database already contains products. Skipping product seed.");
        }
    }
}