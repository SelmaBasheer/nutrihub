using MediatR;


namespace CatalogService.Application.Commands.ProductCommands.UpdateProduct
{
    public record UpdateProductCommand
    (
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Stock,
        Guid CategoryId,
        List<string> ImageUrls
    ) : IRequest<Unit>;
}
