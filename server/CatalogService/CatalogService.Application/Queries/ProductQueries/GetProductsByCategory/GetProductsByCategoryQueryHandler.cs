using CatalogService.Application.DTOs;
using CatalogService.Application.Queries.ProductQueries.GetProductById;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;

namespace CatalogService.Application.Queries.ProductQueries.GetProductsByCategory
{
    public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger = Log.ForContext<GetProductByIdQueryHandler>();

        public GetProductsByCategoryQueryHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching products by category with id {CategoryId}", request.CategoryId);

            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
            if (category is null)
            {
                _logger.Warning("Category not found {CategoryId}", request.CategoryId);
                throw new KeyNotFoundException($"Category with ID {request.CategoryId} not found.");
            }

            var products = await _productRepository.GetProductsByCategoryAsync(request.CategoryId, cancellationToken);

            return products.Select(p => new ProductDto
            (
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                p.CategoryId,
                p.ImageUrls,
                p.IsAvailable,
                p.CreatedAt,
                p.UpdatedAt
            )).ToList();
        }
    }
}
