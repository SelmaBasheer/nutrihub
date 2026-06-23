using CatalogService.Domain.Entities;
using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.Persistence;
using MongoDB.Driver;
using Serilog;

namespace CatalogService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        private readonly ILogger _logger = Log.ForContext<ProductRepository>();

        public ProductRepository(CatalogDbContext context)
        {
            _products = context.Products;
        }

        public async Task AddProductAsync(Product product, CancellationToken ct = default)
        {
            _logger.Information("Adding product {ProductId} to database", product.Id);
            await _products.InsertOneAsync(product, cancellationToken: ct);
        }

        public async Task DeleteProductAsync(Guid id, CancellationToken ct = default)
        {
            _logger.Information("Deleting product {ProductId} from database", id);
            await _products.DeleteOneAsync(p => p.Id == id, ct);
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync(CancellationToken ct = default)
        {
            _logger.Information("Fetching all Products");
            return await _products.Find(_ => true).ToListAsync(ct);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken ct = default)
        {
            _logger.Information("Fetching category {CategoryId} from database", categoryId);
            return await _products.Find(p => p.CategoryId == categoryId).ToListAsync(ct);   
        }

        public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken ct = default)
        {
            _logger.Information("Fetching product {ProductId} from database", id);
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync(ct); 
        }

        public async Task<bool> IsProductExistAsync(string name, CancellationToken ct = default)
        {
            return await _products.Find(p => p.Name == name).AnyAsync(ct);  
        }

        public async Task UpdateProductAsync(Product product, CancellationToken ct = default)
        {
            await _products.ReplaceOneAsync(p => p.Id == product.Id, product, cancellationToken : ct);
        }
    }
}
