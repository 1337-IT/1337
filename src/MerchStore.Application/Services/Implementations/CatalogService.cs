using MerchStore.Application.Services.Interfaces;
using MerchStore.Domain.Entities;
using MerchStore.Domain.Interfaces;

namespace MerchStore.Application.Services.Implementations;

public class CatalogService : ICatalogService
{
    private readonly IProductRepository _productRepository;

    public CatalogService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _productRepository.GetByIdAsync(productId);
    }
    public async Task AddProductAsync(Product product)
    {
        await _productRepository.AddAsync(product);
    }
}
