using MediatR;


namespace CatalogService.Application.Commands.CategoryCommands.DeleteCategory
{
    public record DeleteCategoryCommand
    (
        Guid Id
    ) : IRequest<Unit>;
}
