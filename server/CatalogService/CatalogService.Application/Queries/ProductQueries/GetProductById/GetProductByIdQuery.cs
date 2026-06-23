using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Application.Queries.ProductQueries.GetProductById
{
    public record GetProductByIdQuery
    (
        Guid Id
    ) : IRequest<ProductDto>;
}
