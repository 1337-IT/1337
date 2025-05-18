using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MerchStore.Domain.Entities;

namespace MerchStore.Infrastructure.Persistence;

/// <summary>
/// Class for seeding the database with initial data.
/// This is useful for development, testing, and demos.
/// </summary>
public class AppDbContextSeeder
{
    private readonly ILogger<AppDbContextSeeder> _logger;
    private readonly AppDbContext _context;

    public AppDbContextSeeder(AppDbContext context, ILogger<AppDbContextSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public virtual async Task SeedAsync()
    {
        try
        {
            await _context.Database.EnsureCreatedAsync();
            await SeedProductsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedProductsAsync()
    {
        if (!await _context.Products.AnyAsync())
        {
            _logger.LogInformation("Seeding products...");

            var products = new List<Product>
            {
                new Product(
                    "Casual Sports Cap - Green Edition",
                    "A comfortable cap suits all weather conditions.",
                    new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap1.jpg"),
                    99.00m,
                    10),

                new Product(
                    "Casual Sports Cap - Grey Edition",
                    "A comfortable cap suits all weather conditions.",
                    new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap2.jpg"),
                    109.00m,
                    20),

                new Product(
                    "Stylish Sports Cap - Fire Edition",
                    "A Comfortable cap suits while going out for Sport events.",
                    new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap3.jpg"),
                    99.00m,
                    0),

                new Product(
                    "Stylish Sports Cap - Minimalist Design",
                    "A Comfortable cap suits while going out for events.",
                    new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap4.jpg"),
                    109.99m,
                    15),

                new Product(
                    "Cowboy Hat featuring a 6 Logo",
                    "A western charm with modern attitude.",
                    new Uri("https://merchstoreblobstorage.blob.core.windows.net/hats/CowboyCream.png"),
                    159.99m,
                    15),

                new Product(
                    "Sleek Black Cap Featuring Customized Logo",
                    "Your style, your logo — fully customizable to make it uniquely yours.",
                    new Uri("https://merchstoreblobstorage.blob.core.windows.net/hats/Customized%20logo%20black.png"),
                    199.99m,
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
