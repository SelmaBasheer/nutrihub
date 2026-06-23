using CatalogService.Application.DTOs;
using MediatR;


namespace CatalogService.Application.Queries.CategoryQueries.GetCategoryById
{
    public record GetCategoryByIdQuery
    (
        Guid Id
    ) : IRequest<CategoryDto>;
}
