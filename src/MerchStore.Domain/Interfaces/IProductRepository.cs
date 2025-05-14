using MerchStore.Domain.Entities;

namespace MerchStore.Domain.Interfaces;

/// <summary>
/// Defines product-specific data access operations.
/// </summary>
public interface IProductRepository : IRepository<Product, Guid>
{
    // Add any product-specific methods here in the future
    // Example:
    // Task<IEnumerable<Product>> GetTopSellingProductsAsync();
}
