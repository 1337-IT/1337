using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MerchStore.Domain.Interfaces;
using MerchStore.Infrastructure.Persistence.Repositories;
using MerchStore.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using MerchStore.Infrastructure.ExternalServices.Reviews;
using MerchStore.Application.Common.Interfaces; // ✅ For ICatalogSeeder
using MerchStore.Infrastructure.Seeders;        // ✅ For CatalogSeeder
using MerchStore.Infrastructure.ExternalServices; // ✅ For BlobStorageService

namespace MerchStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CosmosDbSettings>(configuration.GetSection("CosmosDbSettings"));
        services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage")); // ✅ Add blob storage config

        var useInMemory = configuration.GetValue<bool>("UseInMemoryDb");

        if (useInMemory)
        {
            // 🔁 In-memory repo for development/testing
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        }
        else
        {
            // 🌐 Cosmos DB for production
            services.AddScoped<IProductRepository, CosmosProductRepository>();
        }

        // Register review dependencies
        services.AddScoped<IReviewRepository, ExternalReviewRepository>();
        services.AddScoped<MockReviewService>();
        services.AddScoped<ReviewApiClient>();

        // ✅ Register seeder here
        services.AddScoped<ICatalogSeeder, CatalogSeeder>();

        // ✅ Register blob upload service
        services.AddScoped<BlobStorageService>();

        return services;
    }
}
