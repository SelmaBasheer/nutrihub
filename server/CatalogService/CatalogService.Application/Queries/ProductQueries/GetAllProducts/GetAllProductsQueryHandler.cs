using CatalogService.Application.DTOs;
using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;

namespace CatalogService.Application.Queries.ProductQueries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger = Log.ForContext<GetAllProductsQueryHandler>();

        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all products.");

            var products = await _productRepository.GetAllProductAsync(cancellationToken);

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
