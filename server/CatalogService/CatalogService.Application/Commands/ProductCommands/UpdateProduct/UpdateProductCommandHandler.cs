using CatalogService.Domain.Entities;
using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;


namespace CatalogService.Application.Commands.ProductCommands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger = Log.ForContext<UpdateProductCommandHandler>();

        public UpdateProductCommandHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.Information("Updating product with id {ProductId}", request.Id);

            var product = await _productRepository.GetProductByIdAsync(request.Id, cancellationToken);
            if (product is null)
            {
                _logger.Warning("Update failed — product not found {ProductId}", request.Id);
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }

            var categoryExists = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
            if (categoryExists is null)
            {
                _logger.Warning("Update failed — category not found {CategoryId}", request.CategoryId);
                throw new KeyNotFoundException($"Category with ID {request.CategoryId} not found.");
            }

            product.Update(request.Name, request.Description, request.Price, request.CategoryId, request.ImageUrls);
            await _productRepository.UpdateProductAsync(product, cancellationToken);

            _logger.Information("Product updated successfully {ProductId}", request.Id);
            return Unit.Value;
        }
    }
}
