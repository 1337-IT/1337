using MerchStore.Domain.Entities;
using MerchStore.Domain.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MerchStore.Infrastructure.Persistence.Repositories
{
    public class CosmosDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }

    public class CosmosProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public CosmosProductRepository(IOptions<CosmosDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _products = database.GetCollection<Product>(settings.Value.CollectionName);
        }

        public async Task AddAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        public async Task RemoveAsync(Product product)
        {
            await _products.DeleteOneAsync(p => p.Id == product.Id);
        }

        public async Task UpdateAsync(Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
        }
    }
}
