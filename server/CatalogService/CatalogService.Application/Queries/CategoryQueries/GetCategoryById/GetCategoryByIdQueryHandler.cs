using CatalogService.Application.DTOs;
using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;

namespace CatalogService.Application.Queries.CategoryQueries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger = Log.ForContext<GetCategoryByIdQueryHandler>();

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching category {CategoryId}", request.Id);

            var category = await _categoryRepository.GetCategoryByIdAsync(request.Id, cancellationToken);

            if (category is null)
            {
                _logger.Warning("Category not found {CategoryId}", request.Id);
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            }

            return new CategoryDto(
                category.Id,
                category.Name,
                category.Description,
                category.ImageUrl,
                category.IsActive,
                category.CreatedAt
            );
        }
    }
}
