using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;

namespace CatalogService.Application.Commands.CategoryCommands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger = Log.ForContext<UpdateCategoryCommand>();

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.Information("Updating category with id {Id}", request.Id);

            var category = await _categoryRepository.GetCategoryByIdAsync(request.Id);
            if (category is null)
            {
                _logger.Warning("Update failed — category not found {CategoryId}", request.Id);
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            }

            //checks if the category already exists
            var exists = await _categoryRepository.IsCategoryExistAsync(request.Name, cancellationToken);
            if (exists)
            {
                _logger.Warning("Category updation failed — category name already exists {Name}", request.Name);
                throw new InvalidOperationException("Category with this name already exists");
            }

            //Update category using factory method
            category.Update(request.Name, request.Description, request.ImageUrl);
            await _categoryRepository.UpdateCategoryAsync(category);

            _logger.Information("Category updated successfully {CategoryId}", request.Id);
            return Unit.Value;
        }
    }
}
