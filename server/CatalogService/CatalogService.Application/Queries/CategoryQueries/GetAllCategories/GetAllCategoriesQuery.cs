using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Application.Queries.CategoryQueries.GetAllCategories
{
    public record GetAllCategoriesQuery() : IRequest<List<CategoryDto>>;
}
