using MediatR;


namespace CatalogService.Application.Commands.CategoryCommands.CreateCategory
{
    public record CreateCategoryCommand
    (
        string Name,
        string Description,
        string ImageUrl
    ) : IRequest<Guid>;
}
