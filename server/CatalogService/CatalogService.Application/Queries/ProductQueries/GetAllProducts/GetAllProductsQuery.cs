using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Application.Queries.ProductQueries.GetAllProducts
{
    public record GetAllProductsQuery() : IRequest<List<ProductDto>>;
}
