using MediatR;


namespace CatalogService.Application.Commands.ProductCommands.DeleteProduct
{
    public record DeleteProductCommand
    (
        Guid Id
    ) : IRequest<Unit>;
}
