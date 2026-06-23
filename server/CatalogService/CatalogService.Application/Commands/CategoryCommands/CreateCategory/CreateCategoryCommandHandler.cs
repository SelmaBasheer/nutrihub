using CatalogService.Domain.Entities;
using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;


namespace CatalogService.Application.Commands.CategoryCommands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger = Log.ForContext<CreateCategoryCommand>();

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.Information("Creating new category with name {Name}", request.Name);

            //checks if the category already exists
            var exists = await _categoryRepository.IsCategoryExistAsync(request.Name, cancellationToken);
            if (exists)
            {
                _logger.Warning("Category creation failed — category name already exists {Name}", request.Name);
                throw new InvalidOperationException("Category with this name already exists");
            }

            //Create category using factory method
            var category = Category.Create(request.Name, request.Description, request.ImageUrl);

            //Save to database
            await _categoryRepository.AddCategoryAsync(category, cancellationToken);

            return category.Id;
        }
    }
}
