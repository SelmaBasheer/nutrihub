using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;


namespace CatalogService.Application.Commands.ProductCommands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger = Log.ForContext<DeleteProductCommandHandler>();

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _logger.Information("Deletion attempt for product with Id {ProductId}", request.Id);

            var product = await _productRepository.GetProductByIdAsync(request.Id);
            if (product is null)
            {
                _logger.Warning("Deletion failed - Product with Id {ProductId} not found.",request.Id);
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }

            await _productRepository.DeleteProductAsync(request.Id, cancellationToken);

            _logger.Information("Product deleted successfully {ProductId}", request.Id);

            return Unit.Value;
        }
    }
}
