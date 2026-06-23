using CatalogService.Application.DTOs;
using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;

namespace CatalogService.Application.Queries.ProductQueries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger = Log.ForContext<GetProductByIdQueryHandler>();

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching product {ProductId}", request.Id);

            var product = await _productRepository.GetProductByIdAsync(request.Id, cancellationToken); 
            if (product is null)
            {
                _logger.Warning("Product not found {ProductId}", request.Id);
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }

            return new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.CategoryId,
                product.ImageUrls,
                product.IsAvailable,
                product.CreatedAt,
                product.UpdatedAt
            );
        }
    }
}
