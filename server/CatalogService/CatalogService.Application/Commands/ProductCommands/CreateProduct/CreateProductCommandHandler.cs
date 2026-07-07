using CatalogService.Application.Contracts;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;


namespace CatalogService.Application.Commands.ProductCommands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IInventoryGrpcClient _inventoryGrpcClient;
        private readonly ILogger _logger = Log.ForContext<CreateProductCommandHandler>();

        public CreateProductCommandHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IInventoryGrpcClient inventoryGrpcClient)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _inventoryGrpcClient = inventoryGrpcClient;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.Information("Creating new product with name { Name}", request.Name);

            var categoryExists = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);

            if (categoryExists is null)
            {
                _logger.Warning("Product creation failed — category not found {CategoryId}", request.CategoryId);
                throw new KeyNotFoundException($"Category with ID {request.CategoryId} not found.");
            }

            var exists = await _productRepository.IsProductExistAsync(request.Name, cancellationToken);
            if (exists)
            {
                _logger.Warning("Product creation failed — product name already exists {Name}", request.Name);
                throw new InvalidOperationException("Product with this name already exists.");
            }

            var product = Product.Create(
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.CategoryId,
                request.ImageUrls
            );

            await _productRepository.AddProductAsync(product);

            await _inventoryGrpcClient.CreateInventoryAsync(
                product.Id,
                product.Name,
                product.Stock
            );

            _logger.Information("Product created successfully {ProductId}", product.Id);
            return product.Id;
        }
    }
}
