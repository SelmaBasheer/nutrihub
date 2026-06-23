using MediatR;

namespace CatalogService.Application.Commands.CategoryCommands.UpdateCategory
{
    public record UpdateCategoryCommand
    (
        Guid Id,
        string Name,
        string Description,
        string ImageUrl
    ) : IRequest<Unit>;
}
