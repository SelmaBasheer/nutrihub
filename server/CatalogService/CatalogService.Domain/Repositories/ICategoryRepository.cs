using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Repositories
{
    public interface ICategoryRepository
    {  
        Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Category>> GetAllCategoryAsync(CancellationToken ct = default);
        Task<bool> IsCategoryExistAsync(string name, CancellationToken ct = default);
        Task AddCategoryAsync(Category category, CancellationToken ct = default);
        Task UpdateCategoryAsync(Category category, CancellationToken ct = default);
        Task DeleteCategoryAsync(Guid id, CancellationToken ct = default);
    }
}
