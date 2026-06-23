using CatalogService.Domain.Entities;
using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;

namespace CatalogService.Application.Commands.CategoryCommands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger = Log.ForContext<DeleteCategoryCommandHandler>();

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.Information("Deleting category {Id} ", request.Id);

            var category = await _categoryRepository.GetCategoryByIdAsync(request.Id);

            if (category is null)
            {
                _logger.Warning("Delete failed — category not found {CategoryId}", request.Id);
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            }

            await _categoryRepository.DeleteCategoryAsync(request.Id, cancellationToken);

            _logger.Information("Category deleted successfully {CategoryId}", request.Id);

            return Unit.Value;
        }
    }
}
