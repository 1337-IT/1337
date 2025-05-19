using MerchStore.Application.Common.Interfaces;
using MerchStore.Domain.Entities;
using MerchStore.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace MerchStore.Infrastructure.Seeders;

public class CatalogSeeder : ICatalogSeeder
{
    private readonly IProductRepository _repo;
    private readonly ILogger<CatalogSeeder> _logger;

    public CatalogSeeder(IProductRepository repo, ILogger<CatalogSeeder> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        var existing = await _repo.GetAllAsync();
        if (existing.Any())
        {
            _logger.LogInformation("✅ Products already exist. Skipping catalog seed.");
            return;
        }

        var products = new List<Product>
        {
            new Product("Casual Sports Cap - Green Edition",
                "A comfortable cap suits all weather conditions.",
                new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap1.jpg"),
                99.00m,
                10),

            new Product("Casual Sports Cap - Grey Edition",
                "A comfortable cap suits all weather conditions.",
                new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap2.jpg"),
                109.00m,
                20),

            new Product("Stylish Sports Cap - Fire Edition",
                "A Comfortable cap suits while going out for Sport events.",
                new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap3.jpg"),
                99.00m,
                0),

            new Product("Stylish Sports Cap - Minimalist Design",
                "A Comfortable cap suits while going out for events.",
                new Uri("https://1337itblob.blob.core.windows.net/merchstore/cap4.jpg"),
                109.99m,
                15),

            new Product("Cowboy Hat featuring a 6 Logo",
                "A western charm with modern attitude.",
                new Uri("https://merchstoreblobstorage.blob.core.windows.net/hats/CowboyCream.png"),
                159.99m,
                15),

            new Product("Sleek Black Cap Featuring Customized Logo",
                "Your style, your logo — fully customizable to make it uniquely yours.",
                new Uri("https://merchstoreblobstorage.blob.core.windows.net/hats/Customized%20logo%20black.png"),
                199.99m,
                15)
        };

        foreach (var product in products)
            await _repo.AddAsync(product);

        _logger.LogInformation("✅ Seeded 6 products into Cosmos DB.");
    }
}

