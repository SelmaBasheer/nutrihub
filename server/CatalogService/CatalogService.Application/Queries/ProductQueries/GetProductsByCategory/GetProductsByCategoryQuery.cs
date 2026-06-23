

using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Application.Queries.ProductQueries.GetProductsByCategory
{
    public record GetProductsByCategoryQuery
    (
        Guid CategoryId
    ) : IRequest<List<ProductDto>>;
}
