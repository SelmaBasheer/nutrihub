using CatalogService.Application.DTOs;
using CatalogService.Domain.Repositories;
using MediatR;
using Serilog;


namespace CatalogService.Application.Queries.CategoryQueries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger = Log.ForContext<GetAllCategoriesQueryHandler>();

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all categories");

            var categories = await _categoryRepository.GetAllCategoryAsync(cancellationToken);

            return categories.Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Description,
                c.ImageUrl,
                c.IsActive,
                c.CreatedAt
            )).ToList();
        }
    }
}
