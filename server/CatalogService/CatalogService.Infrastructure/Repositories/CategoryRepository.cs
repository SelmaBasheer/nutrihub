using CatalogService.Domain.Entities;
using CatalogService.Domain.Repositories;
using CatalogService.Infrastructure.Persistence;
using MongoDB.Driver;
using Serilog;

namespace CatalogService.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;
        private readonly ILogger _logger = Log.ForContext<CategoryRepository>();

        public CategoryRepository(CatalogDbContext context)
        {
            _categories = context.Categories;
        }

        public async Task AddCategoryAsync(Category category, CancellationToken ct = default)
        {
            _logger.Information("Adding category {CategoryId} to database", category.Id);
            await _categories.InsertOneAsync(category, cancellationToken: ct);
        }

        public async Task DeleteCategoryAsync(Guid id, CancellationToken ct = default)
        {
            _logger.Information("Deleting category {CategoryId} from database", id);
            await _categories.DeleteOneAsync(c => c.Id == id, ct);
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync(CancellationToken ct = default)
        {
            _logger.Information("Fetching all Categories");
            return await _categories.Find(_ => true).ToListAsync(ct);
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken ct = default)
        {
            _logger.Information("Fetching category {CategoryId} from database", id);
            return await _categories.Find(c => c.Id == id).FirstOrDefaultAsync(ct);
        }

        public async Task<bool> IsCategoryExistAsync(string name, CancellationToken ct = default)
        {
            return await _categories.Find(c => c.Name == name).AnyAsync(ct);
        }

        public async Task UpdateCategoryAsync(Category category, CancellationToken ct = default)
        {
            _logger.Information("Updating category {CategoryId} in database", category.Id);
            await _categories.ReplaceOneAsync(c => c.Id == category.Id, category, cancellationToken: ct);
        }
    }
}
