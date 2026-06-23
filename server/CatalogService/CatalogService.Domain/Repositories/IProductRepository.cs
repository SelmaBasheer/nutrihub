using CatalogService.Domain.Entities;


namespace CatalogService.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Product>> GetAllProductAsync(CancellationToken ct = default);
        Task<IEnumerable<Product>> GetCategoryByIdAsync(Guid categoryId, CancellationToken ct = default);
        Task<bool> IsProductExistAsync(string name, CancellationToken ct = default);
        Task AddProductAsync(Product product, CancellationToken ct = default);
        Task UpdateProductAsync(Product product, CancellationToken ct = default);
        Task DeleteProductAsync(Guid id, CancellationToken ct = default);
    }
}
