

using MediatR;

namespace CatalogService.Application.Commands.ProductCommands.CreateProduct
{
    public record CreateProductCommand
    (
        string Name,
        string Description,
        decimal Price,
        int Stock,
        Guid CategoryId,
        List<string> ImageUrls
    ) : IRequest<Guid>;
}
